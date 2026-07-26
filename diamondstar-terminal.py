import datetime

print("DiamondStar Terminal v1.0.0")
print("© 2026 DiamondStar. All rights reserved.")
print(datetime.datetime.now())

while True:
    command = input("> ")

    if command == "exit":
        break

    elif command == "help":
        print("Commands:\nhelp <command>\nabout\nhelp\ntime\n")

    elif command == "about":
        print("About DiamondStar\nDiamondStar is a free, beginner-friendly programming language designed to make coding easier to understand. It uses human-readable commands while teaching important programming concepts.\n\nDiamondStar's goal is to help new programmers learn without feeling overwhelmed by complicated syntax. It creates a simple path into the world of coding.\n\nDiamondStar is inspired by Python and other easy-to-learn languages while creating its own unique identity.")

    elif command == "time":
        print("Current time:", datetime.datetime.now())

    elif command == "help say":
        print("say - displays text (use {name} for variables")

    elif command == "help make":
        print("make - creates a variable")

    elif command == "help ask for":
        print("ak for - asks the user for input")

    elif command == "help for":
        print("for - displays a variable's value")

    elif command == "help add":
        print("add - changes a number")

    elif command == "help remove":
        print("remove - removes a number")

    elif command == "help if:":
        print("if: begins a condition")

    elif command == "help checks if":
        print("checks - true when a value contains text")

    elif command == "help try:":
        print("try: - true branch")

    elif command == "help or:":
        print("or: - false branch")

    elif command == "help then:":
        print("then: - end")

    elif command == "help repeat []":
        print("repeat - repeats a block")

    elif command == "help loop []":
        print("loop - repeats until stop")

    elif command == "help stop":
        print("stop - stops the current repeat or loop")

    elif command == "help wait":
        print("wait - pauses for seconds")

    else:
        print(f"✦ Error: {command} is not recognized.")
