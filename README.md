# DiamondStar / DSCE

DiamondStar is a simple, human-readable programming language. Write and run DiamondStar programs in **DSCE** (Diamond Star Code Editor).

## Start DSCE

Open `DSCE.exe`. Type a program in the editor and select **Run**. Program messages appear in the Output panel.

The editor includes line numbers, automatic indentation, and code colors:

- Blue: commands
- Gold: text in quotation marks
- Purple: numbers
- Green: comments

## Your first program

Every DiamondStar program must begin with `box [` and end with `]`.

```text
box [
  ! This is a comment
  say "Hello, world!"
]
```

Comments begin with `!`.

## Commands

| Command | What it does | Example |
|---|---|---|
| `say` | Displays text. | `say "Hello!"` |
| `make` | Creates a variable. | `make score = 0` |
| `ask for` | Asks the person running the program for input. | `ask for name "Your name?"` |
| `for` | Displays a variable's value. | `for score` |
| `add` | Adds a number to a variable. | `add score 2` |
| `remove` | Removes a number from a variable. | `remove score 1` |
| `is` | Compares two values in an `if:` statement. | `if: score is 3` |
| `checks if` | Checks whether a variable contains text. | `if: checks if name "a"` |
| `repeat [number]` | Repeats a block a set number of times. | `repeat [3]` |
| `loop []` | Repeats a block until `stop`. | `loop []` |
| `stop` | Stops the current `repeat` or `loop`. | `stop` |
| `wait` | Pauses for a number of seconds. | `wait 1.5` |

## Variables

Use `make` to create a variable. Put a variable inside `{ }` when you want its value to appear in text.

```text
box [
  make name = "DiamondStar"
  say "Welcome to {name}!"
]
```

## Input

```text
box [
  ask for name "What is your name?"
  say "Hello, {name}!"
]
```

## Conditions

Use `if:` for the condition, `try:` for the true result, `or:` for the false result, and `then:` to end it.

```text
box [
  make score = 3
  if: score is 3
  try:
    say "You got it!"
  or:
    say "Try again."
  then:
]
```

Check whether text is inside a variable:

```text
box [
  make word = "DiamondStar"
  if: checks if word "Star"
  try:
    say "Found it!"
  or:
    say "Not found."
  then:
]
```

## Repeating code

Repeat a block a certain number of times:

```text
box [
  repeat [3]
    say "Again!"
  then:
]
```

Make a loop that runs until `stop`:

```text
box [
  loop []
    say "This runs once."
    stop
  then:
]
```

## Waiting

```text
box [
  say "Starting..."
  wait 2
  say "Two seconds passed."
]
```

For a shorter reference, select **Language guide** inside DSCE.

## Getting a security warning?

Click **More Info**, then click **Run anyway**. Remember, this is **NOT** a virus, so Windows can get a little finicky.
