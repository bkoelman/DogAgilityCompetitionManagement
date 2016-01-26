# Dog Agility Competition Management
## Detect the pressing and releasing of keys on a remote

**User action**
* [K-] = Push down key K
* [K+] = Release key K

**Network packet** 
* ~~data~~ = lost packet

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
| [Cur-] | [Cur] | &darr; [Cur] | State &rarr; [InCurrent], CompetitorSelecting(isCurrent: true) | |
| [Cur+] | []    | &uarr; [Cur]   | CompetitorSelectCanceled(isCurrent: true), State &rarr; [None] | |

*Scenario 2: User is setting current competitor number to 123.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur]       | &darr; [Cur] | State &rarr; [InCurrent], CompetitorSelecting(isCurrent: true) | |
| [1-]   | [Cur] + [1] | &darr; [1]           | DigitReceived(isCurrent: true, number=1) | |
| [1+]   | [Cur]       | &uarr; [1]             | | KeyUp is always ignored |
| [2-]   | [Cur] + [2] | &darr; [2]           | DigitReceived(isCurrent: true, number=12) | |
| [2+]   | [Cur]       | &uarr; [2]             | | |
| [3-]   | [Cur] + [3] | &darr; [3]           | DigitReceived(isCurrent: true, number=123) | |
| [3+]   | [Cur]       | &uarr; [3]             | | |
| [Cur+] | []          | &uarr; [Cur]   | CompetitorSelected(isCurrent: true, number=123), State &rarr; [None] | |

*Scenario 3: User is setting current competitor number to 123, but accidentally also presses and holds the [EnterNextCompetitor] key.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur]               | &darr; [Cur] | State &rarr; [InCurrent], CompetitorSelecting(isCurrent: true) | |
| [Nex-] | [Cur] + [Nex]       | &darr; [Nex] | | Discarded due to state |
| [1-]   | [Cur] + [Nex] + [1] | &darr; [1]           | DigitReceived(isCurrent: true, number=1) | |
| [1+]   | [Cur] + [Nex]       | &uarr; [1]             | | |
| [2-]   | [Cur] + [Nex] + [2] | &darr; [2]           | DigitReceived(isCurrent: true, number=12) | |
| [2+]   | [Cur] + [Nex]       | &uarr; [2]             | | |
| [Nex+] | [Cur]               | &uarr; [Nex]   | | Discarded due to state |
| [3-]   | [Cur] + [3]         | &darr; [3]           | DigitReceived(isCurrent: true, number=123) | |
| [3+]   | [Cur]               | &uarr; [3]             | | |
| [Cur+] | []                  | &uarr; [Cur]   | CompetitorSelected(isCurrent: true, number=123), State &rarr; [None] | |

*Scenario 4: Like previous scenario, but user releases key [EnterCurrentCompetitor] before key [EnterNextCompetitor].*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | [Cur]               | &darr; [Cur] | State &rarr; [InCurrent], CompetitorSelecting(isCurrent: true) | |
| [Nex-] | [Cur] + [Nex]       | &darr; [Nex] | | Discarded due to state |
| [1-]   | [Cur] + [Nex] + [1] | &darr; [1]           | DigitReceived(isCurrent: true, number=1) | |
| [1+]   | [Cur] + [Nex]       | &uarr; [1]             | | |
| [2-]   | [Cur] + [Nex] + [2] | &darr; [2]           | DigitReceived(isCurrent: true, number=12) | |
| [2+]   | [Cur] + [Nex]       | &uarr; [2]             | | |
| [Cur+] | [Nex]               | &uarr; [Cur]   | CompetitorSelected(isCurrent: true, number=12), State &rarr; [None] | |
| [3-]   | [Nex] + [3]         | &darr; [3]           | KeyPressCommand(key: ToggleElimination) | Alternate for key [3] |
| [3+]   | [Nex]               | &uarr; [3]             | | |
| [Nex+] | []                  | &uarr; [Nex]   | | Discarded due to state |

*Scenario 5: Same as previous scenario, but due to an unstable network we miss some packets.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | ~~[Cur]~~           |                        | | |
| [Nex-] | ~~[Cur] + [Nex]~~   |                        | | |
| [1-]   | [Cur] + [Nex] + [1] | &darr; [Nex] | State &rarr; [InNext], CompetitorSelecting(isCurrent: false) | Precedence rule: [Nex], [Cur] |
|        |                     | &darr; [Cur] | | Precedence rule: ModifierDown, KeyDown, KeyUp, ModifierUp |
|        |                     | &darr; [1]           | DigitReceived(isCurrent: false, number=1) | |
| [1+]   | [Cur] + [Nex]       | &uarr; [1]             | | |
| [2-]   | [Cur] + [Nex] + [2] | &darr; [2]           | DigitReceived(isCurrent: false, number=12) | |
| [2+]   | [Cur] + [Nex]       | &uarr; [2]             | | |
| [Cur+] | ~~[Nex]~~           | | | |
| [3-]   | ~~[Nex] + [3]~~     | | | We cannot know about the [3] keypress |
| [3+]   | ~~[Nex]~~           | | | |
| [Nex+] | []                  | &uarr; [Nex]   | CompetitorSelected(isCurrent: false, number=12), State &rarr; [None] | Precedence rule: [Nex], [Cur] |
|        |                     | &uarr; [Cur]   | | Discarded due to state |

*Scenario 6: Due to packet loss, the system incorrectly believes a number is still being entered. User can solve this by pressing the [Cur] key again.*

| User action | Network packet | RemoteKeyTracker | NumberEntryFilter | Remarks |
|:------------|:---------------|:-----------------|:------------------|:--------|
| [Cur-] | ~~[Cur]~~       |                        | | |
| [1-]   | ~~[Cur] + [1]~~ |                        | | We cannot know about the [1] keypress |
| [1+]   | [Cur]           | &darr; [Cur] | State &rarr; [InCurrent], CompetitorSelecting(isCurrent: true) | |
| [Cur+] | ~~[]~~          |                        | | Missed, causing incorrect state below |
| [2-]   | [2]             | &darr; [2]           | DigitReceived(isCurrent: true, number=2) | Should have been [SetIntermediate] |
|        |                 | &uarr; [Cur]   | | |
| [2+]   | []              | &uarr; [2]             | | |
| [Cur-] | [Cur]           | &darr; [Cur] | | Discard, modifier seems already down |
| [Cur+] | []              | &uarr; [Cur]   | CompetitorSelected(isCurrent: true, number=2), State &rarr; [None] | State has been corrected |
