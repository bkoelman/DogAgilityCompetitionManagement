# Dog Agility Competition Management
## Detect the pressing and releasing of keys on a remote

**User action**
* [K-] = Press down key K
* [K+] = Release key K

**Network packet** 
* X = lost packet

**RemoteKeyTracker**
* Performs key change detection
* Stores keys from previous CIRCE operation per device
* Raises Up/Down events for modifiers
* Raises Up/Down events for keys
* Somewhat resilient to lost packets

**NumberEntryFilter**
* Performs building of numbers out of digits
* Keeps track of entered digits per device
* Keeps track for which competitor (current/next/none) per device
* Raises events related to number entry - start/append/stop
* Raises events for commands (key alternate function)

*Scenario 1: User accidentally presses the [Cur] key.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur] | ModifierKeyDown: [Cur] | State := [InCurrent], NotifyCompetitorSelecting(isCurrent: true) | |
| [Cur+] | []    | ModifierKeyUp: [Cur]   | NotifyCompetitorSelectCanceled(isCurrent: true), State := [None] | |

*Scenario 2: User is setting current competitor number to 123.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur]       | ModifierKeyDown: [Cur] | State := [InCurrent], NotifyCompetitorSelecting(isCurrent: true) | |
| [1-]   | [Cur] + [1] | KeyDown: [1]           | NotifyDigitReceived(isCurrent: true, number=1) | |
| [1+]   | [Cur]       | KeyUp: [1]             | | KeyUp is always ignored |
| [2-]   | [Cur] + [2] | KeyDown: [2]           | NotifyDigitReceived(isCurrent: true, number=12) | |
| [2+]   | [Cur]       | KeyUp: [2]             | | |
| [3-]   | [Cur] + [3] | KeyDown: [3]           | NotifyDigitReceived(isCurrent: true, number=123) | |
| [3+]   | [Cur]       | KeyUp: [3]             | | |
| [Cur+] | []          | ModifierKeyUp: [Cur]   | NotifyCompetitorSelected(isCurrent: true, number=123), State := [None] | |

*Scenario 3: User is setting current competitor number to 123, but accidentally also presses and holds the [EnterNextCompetitor] key.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur]               | ModifierKeyDown: [Cur] | State := [InCurrent], NotifyCompetitorSelecting(isCurrent: true) | |
| [Nex-] | [Cur] + [Nex]       | ModifierKeyDown: [Nex] | | Discarded due to state |
| [1-]   | [Cur] + [Nex] + [1] | KeyDown: [1]           | NotifyDigitReceived(isCurrent: true, number=1) | |
| [1+]   | [Cur] + [Nex]       | KeyUp: [1]             | | |
| [2-]   | [Cur] + [Nex] + [2] | KeyDown: [2]           | NotifyDigitReceived(isCurrent: true, number=12) | |
| [2+]   | [Cur] + [Nex]       | KeyUp: [2]             | | |
| [Nex+] | [Cur]               | ModifierKeyUp: [Nex]   | | Discarded due to state |
| [3-]   | [Cur] + [3]         | KeyDown: [3]           | NotifyDigitReceived(isCurrent: true, number=123) | |
| [3+]   | [Cur]               | KeyUp: [3]             | | |
| [Cur+] | []                  | ModifierKeyUp: [Cur]   | NotifyCompetitorSelected(isCurrent: true, number=123), State := [None] | |

*Scenario 4: Like previous scenario, but user releases key [EnterCurrentCompetitor] before key [EnterNextCompetitor].*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur]               | ModifierKeyDown: [Cur] | State := [InCurrent], NotifyCompetitorSelecting(isCurrent: true) | |
| [Nex-] | [Cur] + [Nex]       | ModifierKeyDown: [Nex] | | Discarded due to state |
| [1-]   | [Cur] + [Nex] + [1] | KeyDown: [1]           | NotifyDigitReceived(isCurrent: true, number=1) | |
| [1+]   | [Cur] + [Nex]       | KeyUp: [1]             | | |
| [2-]   | [Cur] + [Nex] + [2] | KeyDown: [2]           | NotifyDigitReceived(isCurrent: true, number=12) | |
| [2+]   | [Cur] + [Nex]       | KeyUp: [2]             | | |
| [Cur+] | [Nex]               | ModifierKeyUp: [Cur]   | NotifyCompetitorSelected(isCurrent: true, number=12), State := [None] | |
| [3-]   | [Nex] + [3]         | KeyDown: [3]           | NotifyKeyPressCommand(key: ToggleElimination) | Alternate for key [3] |
| [3+]   | [Nex]               | KeyUp: [3]             | | |
| [Nex+] | []                  | ModifierKeyUp: [Nex]   | | Discarded due to state |

*Scenario 5: Same as previous scenario, but due to an unstable network we miss some packets.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | X [Cur]             |                        | | |
| [Nex-] | X [Cur] + [Nex]     |                        | | |
| [1-]   | [Cur] + [Nex] + [1] | ModifierKeyDown: [Nex] | State := [InNext], NotifyCompetitorSelecting(isCurrent: false) | Precedence rule: [Nex], [Cur] |
|        |                     | ModifierKeyDown: [Cur] | | Precedence rule: ModifierDown, KeyDown, KeyUp, ModifierUp |
|        |                     | KeyDown: [1]           | NotifyDigitReceived(isCurrent: false, number=1) | |
| [1+]   | [Cur] + [Nex]       | KeyUp: [1]             | | |
| [2-]   | [Cur] + [Nex] + [2] | KeyDown: [2]           | NotifyDigitReceived(isCurrent: false, number=12) | |
| [2+]   | [Cur] + [Nex]       | KeyUp: [2]             | | |
| [Cur+] | X [Nex]             | | | |
| [3-]   | X [Nex] + [3]       | | | We cannot know about the [3] keypress |
| [3+]   | X [Nex]             | | | |
| [Nex+] | []                  | ModifierKeyUp: [Nex]   | NotifyCompetitorSelected(isCurrent: false, number=12), State := [None] | Precedence rule: [Nex], [Cur] |
|        |                     | ModifierKeyUp: [Cur]   | | Discarded due to state |

*Scenario 6: Due to packet loss, the system incorrectly believes a number is still being entered. User can solve this by pressing the [Cur] key again.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | X [Cur]       |                        | | |
| [1-]   | X [Cur] + [1] |                        | | We cannot know about the [1] keypress |
| [1+]   | [Cur]         | ModifierKeyDown: [Cur] | State := [InCurrent], NotifyCompetitorSelecting(isCurrent: true) | |
| [Cur+] | X []          |                        | | Missed, causing incorrect state below |
| [2-]   | [2]           | KeyDown: [2]           | NotifyDigitReceived(isCurrent: true, number=2) | Should have been [SetIntermediate] |
|        |               | ModifierKeyUp: [Cur]   | | |
| [2+]   | []            | KeyUp: [2]             | | |
| [Cur-] | [Cur]         | ModifierKeyDown: [Cur] | | Discard, modifier seems already down |
| [Cur+] | []            | ModifierKeyUp: [Cur]   | NotifyCompetitorSelected(isCurrent: true, number=2), State := [None] | State has been corrected |
