# CIRCE Interface Specification - Version 0.2.1 [DRAFT]

**Table of Contents**

- [Modification history](#modification-history)
- [About this document](#about)
- [CIRCE interface overview](#interface-overview)
- [CIRCE operations](#circe-operations)
	- [Operations from controller to mediator](#operations-from-controller-to-mediator)
	- [Operations from mediator to controller](#operations-from-mediator-to-controller)
	- [CIRCE protocol messages](#circe-protocol-messages)
		- [Message format](#message-format)
			- [Header](#message-format-header)
			- [Data](#message-format-data)
			- [Trailer](#message-format-trailer)
		- [Checksum](#message-checksum)
- [Parameters related to CIRCE operations](#parameters-related-to-circe-operations)
	- [Login (01)](#operation-login)
	- [Logout (02)](#operation-logout)
	- [Alert (03)](#operation-alert)
	- [Network Setup (04)](#operation-network-setup)
	- [Device Setup (05)](#operation-device-setup)
	- [Synchronize Clocks (06)](#operation-synchronize-clocks)
	- [Visualize (07)](#operation-visualize)
	- [Keep Alive (51)](#operation-keep-alive)
	- [Notify Status (52)](#operation-notify-status)
	- [Notify Action (53)](#operation-notify-action)
	- [Log (54)](#operation-log)
- [CIRCE parameters](#circe-parameters)
	- [Parameter types](#parameter-types)
	- [Parameters](#parameters)
- [Status codes](#status-codes)
- [Example sessions](#examples)
	- [Assigning unique network addresses](#example-assigning-unique-network-addresses)
	- [Forming a logical network](#example-forming-a-logical-network)
	- [Synchronizing hardware clocks](#example-synchronizing-hardware-clocks)
	- [Race events during a competition run](#example-race-events-during-run)
- [Protocol versioning](#versioning)


# <a name="modification-history"></a>Modification history

| Revision  | Release date | Author   | Modification                                                                             |
|:----------|:-------------|:---------|:-----------------------------------------------------------------------------------------|
| **0.2.2** | 2016-01-29   | bkoelman | <ul><li>Renamed Timer Value parameter to Primary Timer Value</li><li>Added parameter Secondary Timer Value</li></ul> |
| **0.2.1** | 2015-05-30   | bkoelman | <ul><li>Changed parameter Timer Value into Start Timer</li></ul> |
| **0.2.0** | 2015-03-20   | bkoelman | <ul><li>Reversed values of Eliminated parameter</li><li>Changed all time values to length 6</li><li>Added new optional parameter VersionMismatch</li></ul> |
| **0.1.9** | 2015-03-06   | bkoelman | <ul><li>Added operation Log for hardware debugging</li></ul> |
| **0.1.8** | 2015-01-07   | bkoelman | <ul><li>Renamed "Previous Competitor Placement" parameter to "Previous Placement" for consistency</li></ul> |
| **0.1.7** | 2014-12-13   | bkoelman | <ul><li>Added operation Visualize</li><li>Extended Capabilities and Roles parameters to include Display</li></ul> |
| **0.1.6** | 2014-04-21   | bkoelman | <ul><li>Improved manufacturing example</li><li>Added operation Synchronize Clocks</li><li>Added new optional parameter Clock Synchronization to Notify Status operation</li><li>Added example for clock synchronization</li></ul> |
| **0.1.5** | 2013-04-07   | bkoelman | <ul><li>Changes to Device Setup operation</li><li>Removed mediator status codes 512 + 513, added status code 1, updated manufacturing example</li><li>Removed DeviceType parameter and string parameter type</li><li>Split Keypad capability into Control Keypad and Numeric Keypad</li></ul> |
| **0.1.4** | 2012-11-12   | bkoelman | <ul><li>Changed format of Address parameter type, updated examples</li><li>Added operation Device Setup and mediator status codes 512 + 513, added example</li></ul> |
| **0.1.3** | 2012-09-03   | bkoelman | <ul><li>Changed possible values for parameters Capabilities and Roles, updated length</li><li>Updated description for operations Notify Status and Notify Action</li><li>Added examples</li><li>Added appendix on protocol versioning</li></ul> |
| **0.1.2** | 2012-06-24   | bkoelman | <ul><li>Corrected Max Length of Version parameters</li><li>Added missing String parameter type</li><li>Changed format of Address parameter type to hexadecimal</li></ul> |
| **0.1.1** | 2012-05-30   | bkoelman | <ul><li>Removed request/response table</li><li>Changed Hexadecimal parameters to Integer</li><li>Added section about parameter types</li><li>Changes in version parameters</li><li>Removed version parameter from Login</li><li>Removed Mediator Address parameter</li><li>Added Logout operation</li><li>Improved descriptions of operations</li></ul> |
| **0.1.0** | 2012-05-27   | bkoelman | Initial version |


# <a name="about"></a>About this document

The Dog Agility Competition Management system is designed to facilitate in dog racing competitions, where one competitor at a time is challenged to complete a race course with obstacles. The system consists of hardware devices, such as wireless passage detection sensors and remote controls, and computer applications. The Dog Agility Competition Controller application directs the racing workflow and can be used to visualize progress. It handles real-time communication with a set of wireless devices via the Dog Agility Competition Mediator device.

This document describes the CIRCE interface, which transfers messages between the Dog Agility Competition Controller application and the Dog Agility Competition Mediator device. CIRCE, the Computer Interface to Race Course Equipment protocol, is used by the Dog Agility Competition Management system.

This document is intended for developers and operator personnel involved in daily operations of the Dog Agility Competition Management system.


# <a name="interface-overview"></a>CIRCE interface overview

The Dog Agility Competition Management system includes the CIRCE interface, which transfers messages between the Dog Agility Competition Controller application and the Dog Agility Competition Mediator device. CIRCE is the Computer Interface to Race Course Equipment protocol. The CIRCE interface is designed for sending and retrieving messages over RS-232.

The Dog Agility Competition Controller application (referred to as “controller” in the rest of this document) is interconnected through the CIRCE connection to the Dog Agility Competition Mediator device (referred to as “mediator” in the rest of this document). The main purpose of this interconnection is for the controller to obtain readings from wireless devices, such as passage gates and remote controls. Other kinds of information can also be conveyed over the interconnection, for example, scanning for hardware devices to dynamically form a logical network configuration.

The system architecture discussed in this document thus consists of a controller and a mediator, and the purpose of this document is to specify the interface between the two.

The CIRCE operations are specified in *[CIRCE operations](#circe-operations)* and *[Parameters related to CIRCE operations](#parameters-related-to-circe-operations)*. Each CIRCE operation carries a number of parameters with it, that is, data items specifying sensor data, some facts about the operation itself, and so on.

The coding of information related to the operations and parameters, that is, how the controller communicates with the mediator, is introduced in *[CIRCE protocol messages](#circe-protocol-messages)*.

The parameters are specified in *[Parameters related to CIRCE operations](#parameters-related-to-circe-operations)*.

In this interface specification, the operations and the parameters specified for each operation represent the maximum amount of information the controller application and the mediator device may provide. It is very important to notice that in some cases it is not reasonable to send all the possible parameters.


# <a name="circe-operations"></a>CIRCE operations

The operations are divided into operations originated by the controller and operations originated by the mediator. The parameters related to each operation are specified in *[Parameters related to CIRCE operations](#parameters-related-to-circe-operations)*, and the values of the parameters are specified in *[CIRCE parameters](#circe-parameters)*.

## <a name="operations-from-controller-to-mediator"></a>Operations from controller to mediator

The following table lists the operations, including their operation code, that are possible from a controller application to a mediator device.

**Table: Operations from controller to mediator**

| Operation               | Description                                                                                                                             |
|:------------------------|:---------------------------------------------------------------------------------------------------------------------------------------|
| Login (01)              | This operation is used by a controller to start communications with a mediator. It is sent before any operations.                      |
| Logout (02)             | This operation is used by a controller to end communications with a mediator.                                                          |
| Alert (03)              | This operation can be used by a controller to activate a presence indication on a wireless device.                                     |
| Network Setup (04)      | This operation is used by a controller to form a logical network configuration of devices.                                             |
| Device Setup (05)       | This operation is used by a controller during the manufacturing process of a new hardware device to assign its unique network address. |
| Synchronize Clocks (06) | This operation is used by a controller to synchronize the clocks of all wireless devices in the logical network.                       |
| Visualize (07)          | This operation is used by a controller to change visualization elements in wireless display devices in the logical network.            |

## <a name="operations-from-mediator-to-controller"></a>Operations from mediator to controller

The following table lists the operations, including their operation code, that are possible from a mediator device to a controller application.

**Table: Operations from mediator to controller**

| Operation          | Description                                                                                                                |
|:-------------------|:--------------------------------------------------------------------------------------------------------------------------|
| Keep Alive (51)    | This operation is used by a mediator to respond to login and to keep the connection alive.                                |
| Notify Status (52) | This operation is used by a mediator to describe the current status of a single device in the wireless network.           |
| Notify Action (53) | This operation is used by a mediator to notify about an activity that occurred in the logical network configuration. For instance, when a passage gate is signaled or a key on a remote control has been pressed. |
| Log (54)           | This operation can be used by a mediator to communicate its internal state and is intended for technical troubleshooting. |

## <a name="circe-protocol-messages"></a>CIRCE protocol messages

### <a name="message-format"></a>Message format

Each message, or operation consists of a header, data, and trailer part of the message:

| Header                  | Parameter list              | Trailer       |
|-------------------------|-----------------------------|---------------|
| &lt;STX&gt;ZZ&lt;TAB&gt;|... PPP:VALUE&lt;TAB&gt; ... | CC&lt;ETX&gt; |

Where:
* `<STX>` = Start of packet
* `ZZ` = Operation code
* `<TAB>` = Delimiter
* `PPP` = Parameter code
* `VALUE` = Parameter value
* `CC` = Checksum
* `<ETX>` = End of packet

The coding of the message parts is explained in *[Header](#message-format-header)*, *[Data](#message-format-data)* and *[Trailer](#message-format-trailer)*.

***Note:*** *Any data transmitted between packets should be ignored. This data can originate from unstable plugs or unreliable wires.*


#### <a name="message-format-header"></a>Header

The header has the following format:

```
<STX>ZZ<TAB>
```

where

- `<STX>` is the start-of-packet indicator, which consists of a single byte containing the decimal value 2.
- `ZZ` defines the operation code and consists of two bytes containing the ASCII characters of the digits 0 to 9, which range from 48 to 57.

The header is terminated by one byte containing the ASCII code of `<TAB>`, which is 9.

An example of the decimal values for each byte of a header is given below:

```
2 48 49 9
```

In the notation used in the rest of this document, the header looks as follows:

```
<STX>01<TAB>
```

#### <a name="message-format-data"></a>Data

The data field consists of a list of parameters each terminated by the `<TAB>` character. All parameter fields have the following format:

```
PPP:Value of the parameter<TAB>
```

where `PPP` indicates the parameter type and consists of 3 bytes containing the ASCII values of the digits 0 to 9 (values 48 to 57). After the single byte containing the ASCII value for the colon (58) the value of the parameter is coded with a variable number of bytes. The parameter is terminated by a single byte containing the ASCII value for TAB (9).

The coding of the parameter value is dependent on the type of the parameter and is explained in *[CIRCE parameters](#circe-parameters)*.

All parameters consist of the ASCII equivalents of digits or the characters of the alphabet. The reserved characters 0x00 (NUL) 0x02 (STX), 0x03 (ETX), 0x09 (TAB) are not allowed in any parameter.

The parameters allowed in a packet depends on the operation. The order of parameters is free, and many parameters may be omitted.

Examples of the decimal values of the bytes for a few parameters are given below, together with the notation used in this document.

- Example: Capabilities parameter (19)

  ```
  48 49 57 58 55 9
  019:7<TAB>
  ```

- Example: Destination Address parameter (14)

  ```
  48 49 52 58 49 55 68 67 65 48 49 9
  014:7DCA01<TAB>
  ```

#### <a name="message-format-trailer"></a>Trailer

The format of the trailer of a packet is:

```
CC<ETX>
```

where `CC` consists of two bytes containing the checksum of the packet, and `<ETX>` is a single byte containing the end-of-packet character, which has the value 3.

The use of the `CC` field is optional, in which case the trailer merely consists of the single `<ETX>` byte.

An example of the decimal values of the bytes in a trailer is given below, together with the notation used in this document.

- Example: Decimal values of the bytes in a trailer

  ```
  51 65 3
  3A<ETX>
  ```
  
All parts combined give the following typical message:

```
<STX>ZZ<TAB>PPP:parameter1<TAB>QQQ:parameter2<TAB><ETX>
```

When real values for `ZZ`, `PPP` and so on, are used, you get a message such as:

```
<STX>01<TAB><ETX>
```

or:

```
<STX>04<TAB>014:7DCA02<TAB>017:1<TAB>020:4<TAB><ETX>
```

### <a name="message-checksum"></a>Checksum

The checksum is an optional field, but when it is used you can calculate it according to the following procedure:

1.  At the beginning of the message, set the checksum to 0.

2.  Retrieve the first byte of the message.

3.  Add the value of the byte to the checksum.

4.  Truncate the checksum so that it contains only the least significant byte.

5.  If available, retrieve the next byte from the message and repeat from step 3. The process stops when the `<ETX>` field is found 2 bytes further in the message.

All characters from the first character to the last character before the checksum characters are included in the sum. Thus, `<STX>` is the first character in the checksum calculation and the last `<TAB>` before the checksum is the last character. The checksum characters and the `<ETX>` are not included in the calculation.

The following line indicates the characters included in the checksum calculation:

```
<STX>ZZ<TAB>PPP:value1<TAB>QQQ:value2<TAB>
```

The coding of the checksum value into the two bytes of field `CC` is done as follows. The most significant 4 bits of the checksum are coded in the first byte and the least significant 4 bits are coded into the second byte of the checksum field `CC`. The ASCII representation of the digits "0" to "9" and "A" to "F" are used for coding the hexadecimal value of the four bits into the message.

For example, if the checksum is 58 (decimal) which is 0x3A (hexadecimal), the most significant 4 bits give us the value 3, and the ASCII representation "3" has the value 51 (decimal) or 0x33 (hex). The second value gets the value "A" which is 65 (decimal).


# <a name="parameters-related-to-circe-operations"></a>Parameters related to CIRCE operations

The following table lists the symbols used. See descriptions for the parameters in *[CIRCE parameters](#circe-parameters)*.

**Table: Parameter symbols**

| Symbol | Meaning             |
|:-------|:--------------------|
| M      | Mandatory parameter |
| O      | Optional parameter  |

## <a name="operation-login"></a>Login (01)

Login must be the first operation sent by a controller to initialize communications with a mediator. The mediator responds by sending a *[Keep Alive (51)](#operation-keep-alive)* operation. This enables a controller to detect at which physical port a mediator is connected and rescan ports when the connection has been interrupted.

The login operation does not contain any parameters.

## <a name="operation-logout"></a>Logout (02)

This operation is sent by a controller to terminate communications. After receiving this operation, the mediator should not send any more operations until another *[Login (01)](#operation-login)* operation has been received.

The logout operation does not contain any parameters.

## <a name="operation-alert"></a>Alert (03)

This operation can be used by a controller to activate a presence indication on a wireless device.

**Table: Alert parameters**

| Number | Name                | Cardinality |
|:-------|:--------------------|:------------|
| 014    | Destination Address | 1           |

## <a name="operation-network-setup"></a>Network Setup (04)

This operation is used by a controller to form a logical network configuration of devices.

A controller sends this operation to instruct a wireless device to join or leave the current logical network configuration, and/or to change assigned roles.

**Table: Network Setup parameters**

| Number | Name                | Cardinality |
|:-------|:--------------------|:------------|
| 014    | Destination Address | 1           |
| 017    | Set Membership      | 1           |
| 020    | Roles               | 1           |

Although a controller should validate its logical network configuration before it allows starting a competition run, nothing prevents another mediator to “take over” a device in the middle of a run.

A mediator must discard any changes in assigned roles when the Set Membership parameter indicates to leave the network, to prevent altering another logical network configuration.

## <a name="operation-device-setup"></a>Device Setup (05)

This operation is used by a controller during the manufacturing process of a mediator or other wireless hardware device to assign its unique network address and capabilities.

A controller may send this operation after login. Upon receipt of this operation, the mediator assigns the new address to itself when the Capabilities parameter is missing. Otherwise, it assigns the new address to a wireless device. When the controller omits the Destination Address, the special address `000000` (see *[Parameter types](#parameter-types)*) is assumed.

**Table: Device Setup parameters**

| Number | Name                | Cardinality |
|:-------|:--------------------|:------------|
| 014    | Destination Address | 0..1        |
| 016    | Assign Address      | 1           |
| 019    | Capabilities        | 0..1        |

## <a name="operation-synchronize-clocks"></a>Synchronize Clocks (06)

This operation is periodically used by a controller to synchronize the hardware clocks of all wireless devices in the logical network. The controller can determine whether the synchronization succeeded by waiting for three seconds on incoming Notify Status (52) operations from devices, which are expected to include the Clock Synchronization parameter with value Sync Succeeded.

This operation does not contain any parameters.

## <a name="operation-visualize"></a>Visualize (07)

This operation is used by a controller during a competition run, to change one or more visualization elements in wireless display devices in the logical network.

Note that for bandwidth efficiency, only changes must be sent by a controller. For all parameters omitted, the receiving display device must leave the associated visualization elements unaffected.

The controller may choose to include the Destination Address parameter multiple times, each time with the unique network address of the specific display device.

The Eliminated parameter has lowest precedence. The Start Timer parameter has precedence over the Primary Timer Value parameter. The Primary Timer Value parameterhas precedence over the Secondary Timer Value parameter. The controller must send parameters with lowest precedence first and prevent conflicts.

***Note:*** *Due to hardware constraints in packet length, currently this operation is sent for each destination individually.*


**Table: Visualize parameters**

| Number | Name                      | Cardinality |
|:-------|:--------------------------|:------------|
| 014    | Destination Address       | 1..n        |
| 028    | Current Competitor Number | 0..1        |
| 029    | Next Competitor Number    | 0..1        |
| 030    | Start Timer               | 0..1        |
| 031    | Primary Timer Value       | 0..1        |
| 032    | Fault Count               | 0..1        |
| 033    | Refusal Count             | 0..1        |
| 034    | Eliminated                | 0..1        |
| 035    | Previous Placement        | 0..1        |
| 038    | Secondary Timer Value     | 0..1        |

## <a name="operation-keep-alive"></a>Keep Alive (51)

This operation is sent by the mediator each time more than a whole second has elapsed since it sent any previous operation. It is also sent by the mediator as a response to the *[Login (01)](#operation-login)* operation.

The controller may choose to end communications by sending a *[Logout (02)](#operation-logout)* operation when the protocol version does not match expectations or when the connection has become idle.

**Table: Keep Alive parameters**

| Number | Name             | Cardinality |
|:-------|:-----------------|:------------|
| 010    | Protocol Version | 1           |
| 012    | Mediator Status  | 1           |

## <a name="operation-notify-status"></a>Notify Status (52)

This operation is periodically sent by a mediator to describe the current status of a single device in the wireless network.

Besides device status display, it can be used by a controller for device discovery and clock synchronization. A device is considered offline when more than three seconds have elapsed since its previous status notification was sent.

**Table: Notify Status parameters**

| Number | Name                  | Cardinality |
|:-------|:----------------------|:------------|
| 015    | Originating Address   | 1           |
| 016    | Get Membership        | 1           |
| 019    | Capabilities          | 1           |
| 020    | Roles                 | 1           |
| 021    | Signal Strength       | 1           |
| 022    | Battery Status        | 0..1        |
| 023    | Is Aligned            | 0..1        |
| 027    | Clock Synchronization | 0..1        |

The capability Time Sensor is mutually exclusive with the Start Sensor, Intermediate Sensor, Finish Sensor and Keypad capabilities. A remote control typically reports the Control Keypad capability. A remote control that includes any of the Start/Intermediate/Finish buttons must include the corresponding capabilities Start/Intermediate/Finish Sensor. A passage gate typically only reports the Time Sensor capability, because it sends time values while being unaware of its meaning.

When a wireless device powers up, after a reboot and when it has joined the logical network, it is expected to include the Clock Synchronization parameter set to Requires Sync until clock synchronization has succeeded. Once the mediator has sent a synchronization request, the wireless device must include the Clock Synchronization parameter with value Sync Succeeded for four seconds.

## <a name="operation-notify-action"></a>Notify Action (53)

This operation is used by a mediator to notify about an activity that occurred in the logical network configuration. For instance, when a passage gate is signaled or a key on a remote control has been pressed.

**Table: Notify Action parameters**

| Number | Name                | Cardinality |
|:-------|:--------------------|:------------|
| 015    | Originating Address | 1           |
| 024    | Input Keys          | 0..1        |
| 025    | Sensor Time         | 0..1        |

Devices with Keypad capabilities must always include the Input Keys parameter. Devices with capability Time Sensor must always include the Sensor Time parameter. A remote control that includes the optional Start/Intermediate/Finish buttons must always include the Sensor Time parameter when any of these keys is pressed.

When a device has a certain capability but is not in a matching role in the current logical network configuration, then the controller must ignore the action. For example, when a remote control includes the capability to provide timings, but it is not configured in any timer role, then any Start/Intermediate/Finish key presses must be discarded by the controller.

## <a name="operation-log"></a>Log (54)

This operation enables a mediator to communicate its internal state to the controller, which in turn facilitates logging to disk. This operation is intended for diagnostic purposes and can be sent at any time, also before login. The single parameter of this operation contains the (variable length) binary data to log.

Note that this operation may be sent conditionally from a mediator, depending on compilation flags or configuration. This operation type must therefore not be used by a controller as an indication that the connection is still alive.

Although there is no technical limit to the payload length, it is not recommended to send more than 255 bytes of payload per operation.

**Table: Log parameters**

| Number | Name     | Cardinality |
|:-------|:---------|:------------|
| 036    | Log Data | 1           |

# <a name="circe-parameters"></a>CIRCE parameters

## <a name="parameter-types"></a>Parameter types

The parameter type defines the allowed values of characters in the CIRCE messages.

**Integer**

Integer is the most common type of parameter. The allowed values are the ASCII representation of the digits 0 to 9. This means that only decimal values from 48 to 57 are allowed.

- Example: Integer parameters

  ```
  021:90<TAB>022:230<TAB>023:1
  ```

**Address**

The Address parameter type is used for passing wireless network addresses of devices to and from the controller. This field always contains exactly six hexadecimal digits. This means that only the values 48 to 57 (digits 0 to 9) and 65 to 70 (characters A to F) are allowed.

The address value `000000` is reserved for unconfigured devices. The address value `FFFFFF` is reserved for internal emulation purposes and should not be used in messages.

The encoded value of an address indicates at which moment in time the associated device has been manufactured. The meaning of the three bytes (after hexadecimal decoding) is described in the next table.

**Table: Encoding of the bits in wireless network addresses**

| Offset | Size (bits) | Values   | Description            |
|:-------|:------------|:---------|:-----------------------|
| 0      | 12          | 0 - 4095 | Year of manufacturing  |
| 12     | 4           | 0 - 15   | Month of manufacturing |
| 16     | 8           | 0 - 255  | Sequence number        |

- Example: Address parameter

  ```
  014:7DCB0E
  ```
  The bits in this address indicate a device that has been manufactured in November 2012 with sequence number 14.

**Version**

The Version parameter type is used for exchanging version numbers in the form X.Y.Z where elements X, Y, and Z are whole numbers in range 0 - 999 (inclusive). This means that besides the values 48 to 57 (digits 0 to 9) also value 46 ('.') is allowed.

- Example: Version parameter

  ```
  010:3.12.439
  ```

**Binary**

The Binary parameter type is used for transferring a binary payload. The payload is encoded in hexadecimal notation. This means that only the values 48 to 57 (digits 0 to 9) and 65 to 70 (characters A to F) are allowed. The payload cannot be empty.

- Example: Binary parameter

  ```
  036:00112233445566778899AABBCCDDEEFF
  ```

## <a name="parameters"></a>Parameters

The table below specifies the CIRCE parameters and their values.

**Table: Parameters**

| Name                      | ID  | Max Length | Type    | Values     | Description |
|:--------------------------|:----|:-----------|:--------|:-----------|:------------|
| Protocol Version          | 010 | 11         | Version | -          | The version number of the CIRCE protocol that is in use. |
| Mediator Status           | 012 | 3          | Integer | 0 - 999    | Indicates internal status of the mediator device. See *[Status codes](#status-codes)* for possible values. |
| Destination Address       | 014 | 6          | Address | -          | Destination address of a device in the wireless network. |
| Originating Address       | 015 | 6          | Address | -          | Originating address of a device in the wireless network. |
| Get Membership            | 016 | 1          | Integer | 0 or 1     | Indicates whether a device is part of the logical network configuration. Values: <ul style="list-style-type: none; padding: 0;"><li>0 - Not in network</li><li>1 - In network</li></ul> |
| Set Membership            | 017 | 1          | Integer | 0 or 1     | Instructs whether a device must become part of the logical network configuration or not. Values: <ul style="list-style-type: none; padding: 0;"><li>0 - Leave network</li><li>1 - Join network</li></ul> |                                                                                          
| Capabilities              | 019 | 3          | Integer | 0 - 127    | The bitmask of capabilities that a device can perform. Possible flags: <ul style="list-style-type: none; padding: 0;"><li>1 - Control keypad</li><li>2 - Numeric keypad</li><li>4 - Start Sensor</li><li>8 - Finish Sensor</li><li>16 - Intermediate Sensor</li><li>32 - Time Sensor</li><li>64 - Display</li></ul> |
| Roles                     | 020 | 3          | Integer | 0 - 127    | The bitmask of roles that are (being) assigned to a device in the logical network. Possible flags: <ul style="list-style-type: none; padding: 0;"><li>1 - Keypad</li><li>2 - Start Timer</li><li>4 - Finish Timer</li><li>8 - Intermediate Timer 1</li><li>16 - Intermediate Timer 2</li><li>32 - Intermediate Timer 3</li><li>64 - Display</li></ul> |
| Signal Strength           | 021 | 3          | Integer | 0 - 255    | The wireless signal strength. Higher values indicate a better signal. |
| Battery Status            | 022 | 3          | Integer | 0 - 255    | Battery status of a device. Higher values indicate longer battery lifetime. |
| Is Aligned                | 023 | 1          | Integer | 0 or 1     | Indicates whether a passage gate is properly aligned. Values: <ul style="list-style-type: none; padding: 0;"><li>0 - Misaligned</li><li>1 - Aligned properly</li></ul> |                                                                                                                          
| Input Keys                | 024 | 5          | Integer | 0 - 65535  | The bitmask of input keys on a keypad that are currently pushed down. Possible flags: <ul style="list-style-type: none; padding: 0;"><li>1 - key 1 / Play Sound A</li><li>2 - key 2 / Pass Intermediate</li><li>4 - key 3 / Toggle Elimination</li><li>8 - key 4</li><li>16 - key 5 / Refusals-</li><li>32 - key 6 / Refusals+</li><li>64 - key 7</li><li>128 - key 8 / Faults-</li><li>256 - key 9 / Faults+</li><li>512 - key Enter Current Competitor</li><li>1024 - key Enter Next Competitor</li><li>2048 - key 0 / Mute Sound</li><li>4096 - key Pass Finish</li><li>8192 - key Pass Start</li><li>16384 - key Reset Run</li><li>32768 - key Ready</li></ul> |
| Sensor Time               | 025 | 6          | Integer | 0 - 999999 | The time (in milliseconds) at which a time sensor detected passage. |
| Assign Address            | 026 | 6          | Address | -          | Network address to be assigned to a new device in the wireless network. |
| Clock Synchronization     | 027 | 1          | Integer | 1 - 2      | Indicates status of synchronization of the hardware clock in a wireless device. Values: <ul style="list-style-type: none; padding: 0;"><li>1 - Requires Sync</li><li>2 - Sync Succeeded</li></ul> |
| Current Competitor Number | 028 | 4          | Integer | 0 - 999    | The value to display for current competitor number, or 0 to hide it. |
| Next Competitor Number    | 029 | 4          | Integer | 0 - 999    | The value to display for next competitor number, or 0 to hide it. |
| Start Timer               | 030 | 1          | Integer | 1          | Resets timer to zero and starts it. |
| Primary Timer Value       | 031 | 6          | Integer | 0 - 999999 | The primary time value (in milliseconds) to display for current competitor, or 999999 to hide it. When used, implicitly stops running timer. |
| Fault Count               | 032 | 2          | Integer | 0 - 99     | The value to display for fault count, or 99 to hide it. |
| Refusal Count             | 033 | 2          | Integer | 0 - 99     | The value to display for refusal count, or 99 to hide it. |
| Eliminated                | 034 | 1          | Integer | 0 or 1     | Toggles display of the elimination indicator. Values: <ul style="list-style-type: none; padding: 0;"><li>0 - Hide elimination indicator</li><li>1 - Display elimination indicator</li></ul> |
| Previous Placement        | 035 | 3          | Integer | 0 - 999    | The value to display for placement of previous competitor, or 0 to hide it. |
| Log Data                  | 036 | -          | Binary  |            | Contains encoded binary data to log. |
| Has Version Mismatch      | 037 | 1          | Integer | 0 or 1     | Indicates whether the version of a device matches the mediator version. Values: <ul style="list-style-type: none; padding: 0;"><li>0 - False (versions match)</li><li>1 - True (versions do not match)</li></ul> |                                                                         
| Secondary Timer Value     | 038 | 6          | Integer | 0 - 999999 | The secondary time value (in milliseconds) to display for current competitor, or 999999 to hide it. |


# <a name="status-codes"></a>Status codes

The table below lists the values and meanings of the parameter Mediator Status (013) for CIRCE connections.

**Table: Mediator Status parameter for CIRCE connections**

| Value | Binary     | Meaning                              |
|:------|:-----------|:-------------------------------------|
| 0     | 0000000000 | Normal mode of operation (no errors) |
| 1     | 0000000001 | Unconfigured mediator                |
| 2     | 0000000002 | Failed to send network packet        |


# <a name="examples"></a>Example sessions

This chapter contains examples of some key usage scenarios.

## <a name="example-assigning-unique-network-addresses"></a>Assigning unique network addresses

The next example session shows how, during the device manufacturing process, a controller assigns a unique network address to a newly constructed mediator, and then to a newly constructed wireless device.

This example session contains the operations *[Login (01)](#operation-login)*, *[Logout (02)](#operation-logout)*, *[Device Setup (05)](#operation-device-setup)*, *[Keep Alive (51)](#operation-keep-alive)* and *[Notify Status (52)](#operation-notify-status)*.

```
Controller <-connect-> Mediator

--> Login
<STX>01<TAB><CC><ETX>

<-- Keep Alive (login response indicates unconfigured)
<STX>51<TAB>010:1.2.34<TAB>012:1<TAB><CC><ETX>

--> Device setup (assigns address 123456 to new mediator)
<STX>05<TAB>026:123456<TAB><CC><ETX>

<-- Keep Alive (indicates configured mediator)
<STX>51<TAB>010:1.2.34<TAB>012:0<TAB><CC><ETX>

--> Device setup (assigns address 222222 and capability TimeSensor to new wireless device)
<STX>05<TAB>026:222222<TAB>019:32<TAB><CC><ETX>

<-- Notify Status (indicates configured wireless device)
<STX>52<TAB>015:222222<TAB>016:0<TAB>019:32<TAB>020:0<TAB>021:150<TAB><CC><ETX>

--> Logout
<STX>02<TAB><CC><ETX>
```

## <a name="example-forming-a-logical-network"></a>Forming a logical network

The example session below shows how the mediator informs the controller about hardware devices in the network. The controller then instructs the mediator how to form its logical wireless network. It is assumed that devices have already been assigned unique network addresses as described in *[Assigning unique network addresses](#example-assigning-unique-network-addresses)*.

This example session contains the operations *[Login (01)](#operation-login)*, *[Alert (03)](#operation-alert)*, *[Network Setup (04)](#operation-network-setup)*, *[Keep Alive (51)](#operation-keep-alive)* and *[Notify Status (52)](#operation-notify-status)*.

```
Controller <-connect-> Mediator

--> Login
<STX>01<TAB><CC><ETX>

<-- Keep Alive (login response)
<STX>51<TAB>010:1.2.34<TAB>012:0<TAB><CC><ETX>

<-- Notify Status (remote control)
<STX>52<TAB>015:AAAAAA<TAB>016:0<TAB>019:1<TAB>020:0<TAB>021:200<TAB><CC><ETX>

<-- Notify Status (first gate)
<STX>52<TAB>015:E1E1E1<TAB>016:0<TAB>019:32<TAB>020:0<TAB>021:150<TAB><CC><ETX>

<-- Notify Status (second gate)
<STX>52<TAB>015:E2E2E2<TAB>016:0<TAB>019:32<TAB>020:0<TAB>021:130<TAB><CC><ETX>

--> Alert (first gate)
<STX>03<TAB>014:E1E1E1<TAB><CC><ETX>

--> Network Setup (remote control joins logical network)
<STX>04<TAB>014:AAAAAA<TAB>017:1<TAB>020:1<TAB><CC><ETX>

--> Network Setup (gate1 joins logical network as start timer)
<STX>04<TAB>014:E1E1E1<TAB>017:1<TAB>020:2<TAB><CC><ETX>

--> Network Setup (gate2 joins logical network as finish timer)
<STX>04<TAB>014:E2E2E2<TAB>017:1<TAB>020:4<TAB><CC><ETX>
```

## <a name="example-synchronizing-hardware-clocks"></a>Synchronizing hardware clocks

At the start of a competition run, all hardware clocks within the logical network need to be synchronized. In this example, the logical network contains two devices. It assumes that a logical network has already been formed as described in *[Forming a logical network](#example-forming-a-logical-network)*.

The example session below contains the operations *[Login (01)](#operation-login)*, *[Synchronize Clocks (06)](#operation-synchronize-clocks)*, *[Keep Alive (51)](#operation-keep-alive)* and *[Notify Status (52)](#operation-notify-status)*.

```
Controller <-connect-> Mediator

--> Login
<STX>01<TAB><CC><ETX>

<-- Keep Alive (login response)
<STX>51<TAB>010:1.2.34<TAB>012:0<TAB><CC><ETX>

<-- Notify Status (remote control)
<STX>52<TAB>015:AAAAAA<TAB>016:0<TAB>019:31<TAB>020:3<TAB>021:200<TAB><CC><ETX>

<-- Notify Status (gate, asking for sync)
<STX>52<TAB>015:E1E1E1<TAB>016:0<TAB>019:32<TAB>020:4<TAB>021:150<TAB>027:1<TAB><CC><ETX>

--> Synchronize clocks
<STX>06<TAB><CC><ETX>

<-- Notify Status (remote control, synced)
<STX>52<TAB>015:AAAAAA<TAB>016:0<TAB>019:31<TAB>020:3<TAB>021:200<TAB>027:2<TAB><CC><ETX>

<-- Notify Status (gate, synced)
<STX>52<TAB>015:E1E1E1<TAB>016:0<TAB>019:32<TAB>020:4<TAB>021:150<TAB>027:2<TAB><CC><ETX>
```

## <a name="example-race-events-during-run"></a>Race events during a competition run

The next example session shows how a single run progresses from preparation to completion. It assumes that a logical network has already been formed as described in *[Forming a logical network](#example-forming-a-logical-network)*. For brevity, clock synchronization is omitted in this example.

This example session contains the operations *[Login (01)](#operation-login)*, *[Keep Alive (51)](#operation-keep-alive)* and *[Notify Action (53)](#operation-notify-action)*.

```
Controller <-connect-> Mediator

--> Login
<STX>01<TAB><CC><ETX>

<-- Keep Alive (login response)
<STX>51<TAB>010:1.2.34<TAB>012:0<TAB><CC><ETX>

<-- Notify Action (Ready key pressed)
<STX>53<TAB>015:AAAAAA<TAB>024:32768<TAB><CC><ETX>
<STX>53<TAB>015:AAAAAA<TAB>024:0<TAB><CC><ETX>

<-- Notify Action (gate1 start timer)
<STX>53<TAB>015:E1E1E1<TAB>025:5000<TAB><CC><ETX>

<-- Notify Action (gate2 finish timer)
<STX>53<TAB>015:E2E2E2<TAB>025:32000<TAB><CC><ETX>
```


# <a name="versioning"></a>Protocol versioning

This document is versioned in the format as described for the Protocol Version (010) parameter. Version numbers of this protocol specification are assigned according to the rules as described below.

The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED", "MAY", and "OPTIONAL" in this document are to be interpreted as described in RFC 2119.

1. A normal version number MUST take the form X.Y.Z where X, Y, and Z are non-negative integers. X is the major version, Y is the minor version, and Z is the release version. Each element MUST increase numerically by increments of one. For instance: 1.9.0 -&gt; 1.10.0 -&gt; 1.11.0.
2. When a major version number is incremented, the minor version and release version MUST be reset to zero. When a minor version number is incremented, the release version MUST be reset to zero. For instance: 1.1.3 -&gt; 2.0.0 and 2.1.7 -&gt; 2.2.0.
3. Once a versioned specification has been released, the contents of that version MUST NOT be modified. Any modifications must be released as a new version.
4. Major version zero (0.y.z) is for initial development. Anything may change at any time. The specification should not be considered stable.
5. Version 1.0.0 defines the formal specification. The way in which the version number is incremented after this release is dependent on this specification and how it changes.
6. Release version Z (x.y.Z | x &gt; 0) MUST be incremented if only backwards compatible changes are introduced. Such changes can be additional explanatory text or corrections in examples, without changing the packet format or definition of operations, parameters and their flow and timings.
7. Minor version Y (x.Y.z | x &gt; 0) MUST be incremented if new, backwards compatible functionality is introduced to the specification. It MUST be incremented if any operations or parameters are marked as deprecated. It MUST be incremented if new operations or parameters are introduced. It MAY include release level changes. Release version MUST be reset to 0 when minor version is incremented.
8. Major version X (X.y.z | X &gt; 0) MUST be incremented if any backwards incompatible changes are introduced to the specification. It MAY include minor and release level changes. Release and minor version MUST be reset to 0 when major version is incremented.
9. Precedence MUST be calculated by separating the version into major, minor and release identifiers in that order. Major, minor, and release versions are always compared numerically. Example: 1.0.0 &lt; 1.0.1 &lt; 1.1.0 &lt; 1.1.1 &lt; 12.1.1 &lt; 12.1.15.
