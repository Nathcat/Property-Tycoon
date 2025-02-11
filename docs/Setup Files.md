# Game Setup Files
Property Tycoon allows you to provide two setup files, which will determine the state of the board,  which cards are available, what actions and data are to be associated with those items.

The data files should be provided in CSV format.

## Board Data
The board data file contains information about each space on the board which the player may land on. Each of these spaces _must_ possess the following data:

 - Position: Integer
 - Name: String

And may _optionally_ have the following data:

 - Group: String
 - Action: <mark>Action String</mark>
 - Cost: Integer

The optional columns should be filled with _null_, if there are unspecified for a given property.

_**Note that the file must contain a number of spaces which is a multiple of 4, and contain at least 4 spaces**_.

### Specifying Rent
Rent should be handled under _Action_, using the <mark>Action String</mark> functions for rent specific to that property type:

 - `PropertyRent(baseRent, oneHouseRent, twoHouseRent, threeHouseRent, fourHouseRent, hotelRent)`
 - `UtilityRent()`
 - `StationRent()`

## Card Data
The card data file contains information about each of the _pot luck_ and _opportunity knocks_ cards which may be drawn during the game. It should contain for the following informaton for every card.

- Description: String
- Group: Either "Pot Luck" or "Opportunity Knocks"
- Action: <mark>Action String</mark>