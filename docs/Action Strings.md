# Action Strings
Some spaces and cards have actions attached to them. _Action Strings_ are a way to specify that action in the game's setup files, allowing the user / developer to easily customise the actions which should be performed when a space is landed on or a card is drawn.

## General Syntax
An Action String is essentially a series of simple commands, which the program will interpret and perform as required. Each command in the string should be separated by a semi-colon.

```
[command] [args...]; [command2] [args2...];
```

Arguments to a command are specified as a list of items, separated by commas, for example:

```
1, 2, 3, 4, 5
```

So a complete action string might be (for example):

```
Fine 10; Move relative -2;
```

As an example of applying a £10 fine to the player, and then moving them two spaces back from their current location.

## Commands
Following are all the commands which may be used in action strings, with details of what they do, and any required arguments.

A command's general syntax will also be specified like so:

```
[command] [arg1: type], [arg2: type], ...;
```

### `Fine`
```
Fine [cash: Integer];
```

Takes `cash` cash from the affected player, and adds that value to the free parking pot.

### `PayOut`
```
PayOut [cash: Integer];
```

Pay the incident player `cash` cash from the bank.

### `Move`
```
Move [relativity: "relative" or "absolute"], [value: Integer];
```

Move the player to the specified location. This location is determined either `relative`ly from their current location, or as an `absolute` space index, for example:

- `Move relative -2;` will move the player two spaces back from their current location.
- `Move relative 2;` will move the player two spaces forward from their current location.
- `Move absolute 0;` will move the player to the space with index `0`.
- `Move absolute 26;` will move the player to the space with index `26`. 

### `PropertyRent`
```
PropertyRent [baseRent: Integer], [oneHouseRent: Integer], [twoHouseRent: Integer], [threeHouseRent: Integer], [fourHouseRent: Integer], [hotelRent: Integer];
```

If the incident property is owned, the player who landed on it will pay the owner rent, based on the values given for each upgrade stage.

### `UtilityRent`
```
UtilityRent;
```

If both utilities are owned by the same player, the player who landed on this space will pay the owner 4 times the amount they rolled on the dice, if both utilities are owned by the same player, then the player who landed on this space will pay the owner 10 times the amount they rolled on the dice.

### `StationRent`
```
StationRent;
```

- If the player who owns the station which was landed on owns 1 station, the player who landed on this station must pay them £25
- If the player who owns the station which was landed on owns 2 stations, the player who landed on this station must pay them £50
- If the player who owns the station which was landed on owns 3 stations, the player who landed on this station must pay them £100
- If the player who owns the station which was landed on owns 4 stations, the player who landed on this station must pay them £200
