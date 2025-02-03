# Property-Tycoon
University of Sussex G4046 Software Engineering 2025 coursework

## Team and individual roles
We are undertaking this coursework as a team of six, consisting of:
- [Rosalina Bowen](https://github.com/Rose-Bowen) - Team Manager, Report, Asset Design
- [Nathan Baines](https://github.com/Nathcat) - Programming, AI, Git Admin
- - [Tyler Gillet](https://github.com/lionbanana) - Programming, AI
- [Brooke Reavell](https://github.com/brooke-ec) - Programming
- [Jaiden Hewitt-Taylor](https://github.com/SpaceDuckie) - Testing
- [Joseph Holland](https://github.com/josephholland07) - Testing, Client Communication

## Project setup
This project is built with Unity 2022.3.39f1. To open the project, [download this editor version](https://unity.com/releases/editor/whats-new/2022.3.39),
clone the repo, and open the project through the Unity Hub.

Pre-built binary executables can be found under this repo's [releases](https://github.com/Nathcat/Property-Tycoon/releases).

# Contribution Guide
For those contributing to this project, find details of how to make such contributions below.

## General rules
- Make changes in a [new branch](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-and-deleting-branches-within-your-repository).
- When you have completed your changes (including testing where possible!) [create a pull request](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request?tool=webui) and provide the following details:
  - What changes you have made
  - What tests you were able to perform
  - If you weren't able to perform some / any tests, why was that?
  - [Link any related issues / pull requests](https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/linking-a-pull-request-to-an-issue)
- Request a review on your pull request before merging (note that merging to main is blocked _until_ a valid review is completed).

## Code style
Please refer to [Unity' s code style tips](https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity) for more information on the styles used in this project.

- Field names should use _Camel Case_
- Method / class names should use _Pascal Case_

Make sure to use meaningful names, and comment you code (more on this shortly).

## Documentation
Make sure to place comments throughout your code where appropriate, C# syntax for comments is as in Java
and other such languages:

```cs
// Single line comment

/*
Multi-line comment
*/
```

Documentation comments should be completed in [Sandcastle XML format](http://ewsoftware.github.io/XMLCommentsGuide/html/4268757F-CE8D-4E6D-8502-4F7F2E22DDA3.htm), for example:

```cs

/// <summary>
///     This is a summary of MyClass!
/// </summary>
public class MyClass { 
    /// <summary>
    ///     This is a summary of MyMethod!
    /// </summary>
    /// <param name="i">This is a description of parameter i!</param>
    /// <returns>This is a description of what MyMethod returns!</returns>
    public int MyMethod(int i) {
        ...
    }
}
```
