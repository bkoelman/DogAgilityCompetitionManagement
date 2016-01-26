# Dog Agility Competition Management
## Design for NumberEntryFilter

*ModifierKeyDown: Current*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | InCurrent<sup>2</sup> | Current<sup>1</sup>        | CompetitorSelecting (isCurrent: true) | Start number building |
| None      | Current        | InCurrent<sup>2</sup> | Current                    | CompetitorSelecting (isCurrent: true) | Invalid (modifier is already down) |
| None      | Next           | InCurrent<sup>2</sup> | Current + Next<sup>1</sup> | CompetitorSelecting (isCurrent: true) | Start number building |
| None      | Current + Next | InCurrent<sup>2</sup> | Current + Next             | CompetitorSelecting (isCurrent: true) | Invalid (modifier is already down) |
| InCurrent | None           | InCurrent             | Current<sup>1</sup>        |  | Join number building |
| InCurrent | Current        | InCurrent             | Current                    |  | Invalid (modifier is already down) |
| InCurrent | Next           | InCurrent             | Current + Next<sup>1</sup> |  |  |
| InCurrent | Current + Next | InCurrent             | Current + Next             |  | Invalid (modifier is already down) |
| InNext    | None           | InNext                | Current<sup>1</sup>        |  |  |
| InNext    | Current        | InNext                | Current                    |  | Invalid (modifier is already down) |
| InNext    | Next           | InNext                | Current + Next<sup>1</sup> |  |  |
| InNext    | Current + Next | InNext                | Current + Next             |  | Invalid (modifier is already down) |

*ModifierKeyDown: Next*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | InNext<sup>2</sup> | Next<sup>1</sup>           | CompetitorSelecting (isCurrent: false) | Start number building |
| None      | Current        | InNext<sup>2</sup> | Current + Next<sup>1</sup> | CompetitorSelecting (isCurrent: false) | Start number building |
| None      | Next           | InNext<sup>2</sup> | Next                       | CompetitorSelecting (isCurrent: false) | Invalid (modifier is already down) |
| None      | Current + Next | InNext<sup>2</sup> | Current + Next             | CompetitorSelecting (isCurrent: false) | Invalid (modifier is already down) |
| InCurrent | None           | InCurrent          | Next<sup>1</sup>           |  | |
| InCurrent | Current        | InCurrent          | Current + Next<sup>1</sup> |  | |
| InCurrent | Next           | InCurrent          | Next                       |  | Invalid (modifier is already down) |
| InCurrent | Current + Next | InCurrent          | Current + Next             |  | Invalid (modifier is already down) |
| InNext    | None           | InNext             | Next<sup>1</sup>           |  | Join number building |
| InNext    | Current        | InNext             | Current + Next<sup>1</sup> |  | |
| InNext    | Next           | InNext             | Next                       |  | Invalid (modifier is already down) |
| InNext    | Current + Next | InNext             | Current + Next             |  | Invalid (modifier is already down) |

**Conclusion - Rules for ModifierKeyDown:**

1. Add incoming modifier to [Local modifiers down] when not already exists.
2. When [Global state (pre)] is None, switch to state that matches incoming modifier and raise event.

---

*ModifierKeyUp: Current*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | None             | None             | | Invalid (modifier was not down) |
| None      | Current        | None             | None<sup>1</sup> | | |
| None      | Next           | None             | Next             | | Invalid (modifier was not down) |
| None      | Current + Next | None             | Next<sup>1</sup> | | |
| InCurrent | None           | None<sup>2</sup> | None             | CompetitorSelected (isCurrent: true) | Invalid (modifier was not down) |
| InCurrent | Current        | None<sup>2</sup> | None<sup>1</sup> | CompetitorSelected (isCurrent: true) | Stop number building |
| InCurrent | Next           | None<sup>2</sup> | Next             | CompetitorSelected (isCurrent: true) | Invalid (modifier was not down) |
| InCurrent | Current + Next | None<sup>2</sup> | Next<sup>1</sup> | CompetitorSelected (isCurrent: true) | Stop number building |
| InNext    | None           | InNext           | None             | | Invalid (modifier was not down) |
| InNext    | Current        | InNext           | None<sup>1</sup> | | |
| InNext    | Next           | InNext           | Next             | | Invalid (modifier was not down) |
| InNext    | Current + Next | InNext           | Next<sup>1</sup> | | |

*ModifierKeyUp: Next*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | None             | None                | | Invalid (modifier was not down) |
| None      | Current        | None             | Current             | | Invalid (modifier was not down) |
| None      | Next           | None             | None<sup>1</sup>    | | |
| None      | Current + Next | None             | Current<sup>1</sup> | | |
| InCurrent | None           | InCurrent        | None                | | Invalid (modifier was not down) |
| InCurrent | Current        | InCurrent        | Current             | | Invalid (modifier was not down) |
| InCurrent | Next           | InCurrent        | None<sup>1</sup>    | | |
| InCurrent | Current + Next | InCurrent        | Current<sup>1</sup> | | |
| InNext    | None           | None<sup>2</sup> | None                | CompetitorSelected (isCurrent: false) | Invalid (modifier was not down) |
| InNext    | Current        | None<sup>2</sup> | Current             | CompetitorSelected (isCurrent: false) | Invalid (modifier was not down) |
| InNext    | Next           | None<sup>2</sup> | None<sup>1</sup>    | CompetitorSelected (isCurrent: false) | Stop number building |
| InNext    | Current + Next | None<sup>2</sup> | Current<sup>1</sup> | CompetitorSelected (isCurrent: false) | Stop number building |

**Conclusion - Rules for ModifierKeyUp:**

1. Remove incoming modifier from [Local modifiers down] when exists.
2. When [Global state (pre)] matches incoming modifier, switch to None and raise event.

---

*KeyDown: 7 (category: digit-only keys)*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | None      | None    | | Key has no command representation |
| None      | Current        | None      | Current | | Number entry is not active |
| None      | Next           | None      | None    | | Number entry is not active |
| None      | Current + Next | None      | Current | | Number entry is not active |
| InCurrent | None           | InCurrent | None    | | Key has no command representation |
| InCurrent | Current        | InCurrent | Current | DigitReceived (isCurrent: true)<sup>1</sup> | Match for number entry |
| InCurrent | Next           | InCurrent | None    | | Incompatible for number entry |
| InCurrent | Current + Next | InCurrent | Current | DigitReceived (isCurrent: true)<sup>1</sup> | Compatible for number entry |
| InNext    | None           | None      | None    | | Key has no command representation |
| InNext    | Current        | None      | Current | | Incompatible for number entry |
| InNext    | Next           | None      | None    | DigitReceived (isCurrent: false)<sup>1</sup> | Match for number entry |
| InNext    | Current + Next | None      | Current | DigitReceived (isCurrent: false)<sup>1</sup> | Compatible for number entry |

*KeyDown: 2 = PassIntermediate (category: multi-functional keys)*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | None      | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| None      | Current        | None      | Current | | Number entry is not active |
| None      | Next           | None      | None    | | Number entry is not active |
| None      | Current + Next | None      | Current | | Number entry is not active |
| InCurrent | None           | InCurrent | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InCurrent | Current        | InCurrent | Current | DigitReceived (isCurrent: true)<sup>1</sup> | Match for number entry |
| InCurrent | Next           | InCurrent | None    | | Incompatible for number entry |
| InCurrent | Current + Next | InCurrent | Current | DigitReceived (isCurrent: true)<sup>1</sup> | Compatible for number entry |
| InNext    | None           | None      | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InNext    | Current        | None      | Current | | Incompatible for number entry |
| InNext    | Next           | None      | None    | DigitReceived (isCurrent: false)<sup>1</sup> | Match for number entry |
| InNext    | Current + Next | None      | Current | DigitReceived (isCurrent: false)<sup>1</sup> | Compatible for number entry |

*KeyDown: PassStart (category: command keys)*

| Global state (pre) | Local keys &darr; (pre) | Global state (post) | Local keys &darr; (post) | Raise event | Comments |
|:-------------------|:------------------------|:--------------------|:-------------------------|:------------|:---------|
| None      | None           | None      | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| None      | Current        | None      | Current | UnknownAction (...)<sup>2</sup> | Key represents a command |
| None      | Next           | None      | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| None      | Current + Next | None      | Current | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InCurrent | None           | InCurrent | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InCurrent | Current        | InCurrent | Current | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InCurrent | Next           | InCurrent | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InCurrent | Current + Next | InCurrent | Current | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InNext    | None           | None      | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InNext    | Current        | None      | Current | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InNext    | Next           | None      | None    | UnknownAction (...)<sup>2</sup> | Key represents a command |
| InNext    | Current + Next | None      | Current | UnknownAction (...)<sup>2</sup> | Key represents a command |

**Conclusion - Rules for KeyDown:**

1. Raise DigitReceived when key category is not command keys -and- a modifier is down that matches global state.
2. Raise UnknownAction when key category is command keys -or- { key category is multi-functional keys -and- no modifiers are down }
