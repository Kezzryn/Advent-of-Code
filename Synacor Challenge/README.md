# Synacor Challenge - developed by [Eric Wastl](https://github.com/topaz) 

## NOTE: As of the time of this commit, the [original site](https://challenge.synacor.com/) appears to have been taken offline. Reddit user [/u/Aneurysm9](https://www.reddit.com/r/adventofcode/comments/11pjsxk/comment/jbzkpo3/) has posted a [repository](https://github.com/Aneurysm9/vm_challenge) with the puzzle binary, documentation and hashes of their solution codes.

### Introduction
In the December of 2022 I learned about the [Advent of Code](https://adventofcode.com/). Advent of Code is a yearly series coding puzzles by Eric Wastl. Those puzzles re-ignited my enjoyment of coding for fun. I've been maintaining a legacy system so these puzzles doubled as a way to modernize my skill set. I choose to learn C# as it aligns with my professional goals.

During my brief time with the Advent of Code (AoC) community I learned of the Synacor Challenge, another puzzle created by the same author. Having finished a block of AoC puzzles, I decided to take a break and spend some time seeing what I could do with the Synacor Challenge.

In the words of the former website: 

>This challenge was given by Synacor at php[tek] and OSCON in years past. Now, we are opening it up to everyone!  
This is a programming challenge, but you can use any language you like. As you complete sections, come back here to check your answer codes.  
The winner during each convention got Apple gift cards and fame. While we're not giving away physical prizes any more, we still hope you'll find the challenges fun and interesting!

The overall setup is straightforward: Given a personalized binary and an architecture sheet, extract the eight codes from the puzzle and submit them to the website.

I've had some minor experience working with binaries, but I've never actually done anything like this before.

Below is my journey, which I hope will be of interest of anybody who started this puzzle with an experience level similar to my own.

***Everything after this is filled with spoilers.***

One final note. All the challenge codes sown below are hashed with MD5. If you take the challenge with my binary, you can compare the hashes of the codes that you've gotten to the codes I've gotten.

Here we go! 

### Code 1 - RTFM!
There are two files provided, `challenge.bin` and `arch-spec`. Leaving the bin file alone, I opened up `arch-spec` and read it over. It provided definitions for the memory, mow math works, some system limits, a few hints, and a listing of the op-codes. The only thing that gave me pause was a comment about the numbers being stored in a little-endian format. I had visions of endless endian bugs. Fortunately this was total non-issue.

Creating a new C# console project, I read through the specification again, then started laying out some basics as I felt out what I would need to implement the program.

The specification states all numbers are unsigned 15 bit integers. I set aside `int` and welcomed my new best friend, `ushort`. `ushort` is a 16 bit number, thus it can contain larger values then the specification permits.

I set some limits to refer to later.
```csharp
const ushort MAX_VALUE = 36768;
const ushort ABSOLUTE_MAX = 36776;
```

There is a block of memory, and a set of 8 registers that are defined as being addressed above main memory.

Main memory and registers map neatly to arrays. `stack` is a `Stack()`. I could see no reason to reinvent the ~~wheel~~ `Stack()`.

I considered merging main memory and the registers into one large array. The addressing seemed to support that idea. I ultimately rejected that option to avoid potential trouble. The spec dictated three memory regions, so I implemented three regions, not two. [Five was right out.](https://montypython.fandom.com/wiki/Holy_Hand_Grenade_of_Antioch)
```csharp
ushort[] mainMemory = new ushort[MAX_VALUE];
ushort[] registers = new ushort[8];
Stack<ushort> stack = new();
```
Finally I'm going to need something to mark what instruction is currently being executed.
```csharp
ushort cursor = 0;
```
In the hints section I found my first code.
> - Here's a code for the challenge website: e36481f9059e4129e68e477b988aab67

With the first code in hand and the first lines of a framework laid out, things were off to a good start.

### Code 2 - Developer's first op-code. 
My next task was to load the `challenge.bin` file into `mainMemory[]`. With a general lack of experience working with binary files, I immediately turned to Microsoft's help documentation. Pilfering some example code, I loaded the binary into `mainMemory[]`.

Almost.

I briefly tripped over the subtle but critical detail that `BaseStream.Position` doesn't directly map to the indexes for `mainMemory[]`. 

After adjusting to account for the offset, the binary successfully loaded into `mainMemory[]`.
```csharp
using BinaryReader r = new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read));
while (r.BaseStream.Position < r.BaseStream.Length)
{
    mainMemory[r.BaseStream.Position / 2] = r.ReadUInt16();
}
```

I still didn't know what I didn't know, so I focused on the suggestion of starting with getting op-codes 0, 19 and 21 working.

Op-code 0 is the "halt" command. I'll need a run loop that would break when encountering op-code 0.

Op-code 19 outputs a character.
```csharp
a = mainMemory[cursor];
Console.Write((char)a);
```
Done! ... as soon as I figure out where to put it.

21 is the "no operation" command. Op-code 21 was an oddly important code to implement. Support for it required the basic framework of read instruction -> process instruction -> repeat.

With that in mind, the immediate goal was to finish off the framework. First up was the main loop to read the instructions from memory. The next task was to build a function that does the real work.

The run loop had the sole responsibility to grab the current instruction with any possible parameters and feed them both to the `DoInstruction()` function. I didn't know how clean the binary would be, so I built in some super awkward bounds checking around the `param` array to ward against array out of bounds checks.

`isRunning` was a global boolean to signal a halt. 

 ```csharp
while (isRunning)
{
    ushort[] param = mainMemory[int.Min(cursor + 1, MAX_VALUE -1)..int.Min(cursor + 4, MAX_VALUE - 1)];

    cursor = DoInstruction(cursor, param); // error handle this, 'cause I bet it blows up one day.
}
```
I love that comment. It never did blow up because over the next day, I refactored the entire param array idea out, never to return.

In `DoCommands()` I built a massive `switch` statement to handle the op-code processing. I filled each code with placeholder statements and the description of the appropriate op-codes from the architecture document.

  ```csharp
ushort DoInstruction(ushort cursor, ushort[] param)
{
    ushort instruction = mainMemory[cursor]; // will throw an error if we're out of bounds, we'll keep that for now. 
    ushort nextInstruction = cursor;

    ushort a = (param.Length >= 1) ? param[0] : (ushort)0;
    ushort b = (param.Length >= 2) ? param[1] : (ushort)0;
    ushort c = (param.Length >= 3) ? param[2] : (ushort)0;

    switch (instruction)
    {
    case 0:     // halt         stop execution and terminate the program
        isRunning = false;
        break;
    case 1:     // set: 1 a b       set register <a> to the value of <b>
        Console.WriteLine($"{instruction} not implemented yet");
        nextInstruction = (ushort)(cursor + 3);
        break;
    case 2:     // push: 2 a	    push <a> onto the stack
        stack.Push(a);
        nextInstruction = (ushort)(cursor + 2);
        break;
```
... and so on for all 22 codes.

There were a couple of things that I knew would need to be addressed.
1) By passing `cursor` as a parameter and then returning it to the run loop, it made `DoInstruction()` responsible for maintaining the state of `cursor`. A better idea would be to pass the instruction directly to `DoInstruction()`. This does not solve the problems of knowing how far to advance `cursor` and how best to communicate the parameters.
2) I have an allergy to magic numbers, so all of those `nextInstruction = (ushort)(cursor + 2);` lines would have to be addressed.
3) I didn't like the need to continually cast to `ushort` for basic math. After some digging, I discovered that `cursor + 2` is `int` math. In this example, `cursor` was cast to be an `int`. 2 was an `int` by default and `+` does `int` math returning an `int`. I might not like casting, but it looks like I'd be stuck with it. 

Still, despite the early roughness of the code, the program loaded, run, and presented this message.

```
Welcome to the Synacor Challenge!
Please record your progress by putting codes like
this one into the challenge website: 7acb6bd7646fa25e711e53f1b4d385f7

Executing self-test...

jmp fails
```
A second code! Progress!

### Code 3 - The Synacor9000() 
This was my first big re-factor. I split out nearly all the code written so far into its own class: `Synacor9000()`. I took the opportunity to rename some functions and variables. For example, `DoCommands()` became `Dispatcher()` and `cursor` became `istrPtr`. I added `Run()` and `Load()` methods. `Dispatcher()` lost the `param` parameter, now accepting only the current instruction.

This reduced the core project down to only a few lines of code.

```csharp
using Synacor_Challenge;
try
{
    const string CHALLENGE_BIN = "challenge.bin";
    Synacor9000 syn9k = new();
    syn9k.Load(new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read)));
    syn9k.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

I implemented a set of helper methods. `Mem_Write()` writes a value to a memory or register address. `Mem_Read()` returns the value from a memory or register address.

The biggest helpers were `Ptr_ReadValue()` and `Ptr_ReadRaw()`.
`Ptr_ReadValue()` returns the value of the current memory address, or the value of a register. `Ptr_ReadRaw()` always returns the value.

For example, if `Ptr_ReadValue()` reads 2134, it will return 2134.  However, if it reads 32770, it will return the contents of register 3.

For the same example, `Ptr_ReadRaw()` would return 32770.



I made an architectural decision. When the `Ptr` methods were called they would advance `instPtr` by one. This simplified my code immensely. Nothing in the program would need to track or communicate anything about any particular op-code's requirements. The act of an op-code requesting data would automatically set the pointer to the proper location for the next request.



I broke the op-codes into smaller methods of related functionality. Each block shared the same basic format, similar to the final form of the one below.
```csharp
private void Op_Comparison(ushort instruction)
    {
        ushort address = Ptr_ReadRaw();
        ushort b = Ptr_ReadValue();
        ushort c = Ptr_ReadValue();

        ushort value = instruction switch
        {
            4 => (b == c) ? USHORT_1 : USHORT_0,        // eq: 4 a b c  set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise    
            5 => (b > c) ? USHORT_1 : USHORT_0,         // gt: 5 a b c  set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
            _ => throw new NotImplementedException()    // Comparison shmarizon.
        };
        Mem_Write(address, value);
    }
```

Op-code implementation went smoothly, each implemented code passed the presented self check until I was left with only three unimplemented op-codes.

Spirits were high, right until I slammed into my first roadblock.

#### The Tale of `rmem` and `wmem` 
> rmem: 15 a b	    read memory at address \<b> and write it to \<a>


> wmem: 16 a b	    write the value from \<b> into memory at address \<a>


At first glance they seem to be the same code. Data goes from \<b> to \<a>. The devil, as the saying goes, is in the details.

Two things conspired to keep me from a solution. 

First, and most importantly, I didn't fully understand the instructions. After flailing for a couple of days, I sought out some help. A reddit post nudged me towards a solution.

For `rmem` the target is a raw value from memory. Like many other op-codes, `address = Ptr_ReadRaw()` would read the correct value.

The source was a whole other animal. "Read memory at address \<b>" This was a two step redirection. I needed to retrieve the value of the memory at the address stored in the memory location of \<b>.

In code terms: `value = Mem_Read(Ptr_ReadValue())`

Once I understood that, it exposed my second issue. At the time, `Ptr_ReadValue()` called `Mem_Read()` directly: `Mem_Read(instPtr++)`. This would give me the value at the address stored in `instPtr`. This wasn't wrong for much of my code, however, it utterly failed for the needs of `rmem`.

To fix it, `Mem_Read()` would read the value from any address passed to it. `Ptr_ReadValue()` would return the value in `mainMemory[]`, unless that value mapped to a register.

After `rmem` got sorted out `wmem` fell into place as a pair of `Ptr_ReadValue()` calls.

*Narrator voice: One week later.*

```csharp
case 15:                        // rmem: 15 a b	    read memory at address <b> and write it to <a>
    address = Ptr_ReadRaw();
    value = Mem_Read(Ptr_ReadValue());
    break;
case 16:                        // wmem: 16 a b	    write the value from <b> into memory at address <a>
    address = Ptr_ReadValue();
    value = Ptr_ReadValue();
    break;
```

Compile, build, run ... 

```
Executing self-test...

self-test complete, all tests pass
The self-test completion code is: ac40c012717b200b04a06587baabdc3b
```

Freakin' *finally.*

### Code 4 - Putting the "I" in "I/O"
The self test is complete and the start of a promised adventure lay before me.
```
What do you do?
>
```
An excellent question with an easy answer. I had one more op-code to implement. 

During my snooping for answers regarding `wmem` I unfortuainly caught a hint of how to handle this. "Input buffer."

C# has three ways to read console input. `Console.Read()`, `Console.ReadKey()`, and `Console.ReadLine()`. These will read a `char`, a keypress and a `string` respectively.

At first glance, `ReadLine()` seemed to be the way to go, and early implementations used it. However, I could not get a consistent experience using it. `Read()` had a similar restriction. `ReadKey()` seemed to gave me the most control over the keyboard input.

I setup two loops. The first loop was in the main program. I created a basic command tree to load and run the challenge file. After starting to run, the main program shell handed off control to a input loop in the `Synacor9000()` class. An "exit" command would pass control back the outer shell.

This snippit was the guts of the op-code key processor. That's ... a bunch.
```csharp
ConsoleKeyInfo cki;
StringBuilder sb = new();
if (string.IsNullOrEmpty(_inputBuffer))
{
    do
    {
        cki = Console.ReadKey();
        switch (cki.Key)
        {
            case ConsoleKey.Escape:
                _stopExecution = true;
                break;
            case ConsoleKey.Backspace:
                sb = sb.Remove(sb.Length - 1, 1);
                // backspace moves the cursor back, so whitespace, then \b to back the cursor up again. 
                Console.Write(" \b");
                break;
            case ConsoleKey.Enter:
                Console.WriteLine();
                sb.Append(NEWLINE);
                break;
            default:
                sb.Append(cki.KeyChar);
                break;
        }
    } while (cki.Key != ConsoleKey.Enter && cki.Key != ConsoleKey.Escape);

    _inputBuffer = string.IsNullOrEmpty(sb.ToString()) || sb.ToString()[0] == NEWLINE ? string.Empty : sb.ToString();
} // end reading from the console. 

if (_stopExecution || string.IsNullOrEmpty(_inputBuffer))
{
    _instPtr--; // back this up to what should be the instruction, so if/when we resume we don't freak out.
    break;
}
else
{
    ushort value = _inputBuffer[0];
    ushort address = Ptr_ReadRaw();

    _inputBuffer = _inputBuffer.Length > 1 ? _inputBuffer[1..] : string.Empty;
                       
    Mem_Write(address, value);
}
```
Take a good look. Nearly all of this would be thrown out in the next big refactor. Right now, I still don't know what I don't know.

Future changes aside, it works and I now have a functional input buffer. Back to the game!

```
Things of interest here:
- tablet

What do you do?
> take tablet

Taken.

What do you do?
> use tablet

You find yourself writing "3f385e97cb9dc86ed1cd10f74184ba2b" on the tablet.  Perhaps it's some kind of code?

What do you do?
>
```
A victory dance, then we go exploring, that's what we do!

### Code 5 - Colossal Code Cave
I lied. I didn't go exploring quite yet. (No comment about dancing.) It was about here that I wrote a `Save()` function into the `Synacor9000()` class.

I identified four items I needed to capture: `mainMemory[]`, `registers[]`, `stack` and `instPtr`.

The two memory arrays were easy to stream out with `BinaryWriter()`. I found that `stack` could be enumerated, which greatly simplified retrieving its state.
`stack` did need to be reversed as I wrote it out, so when I pushed it back during a `Load()` it'd load in the correct order.

As I didn't know the size of the groups, I placed `0xFFFF` markers after each one and then adjusted the `Load()` method accordingly. This allowed the load function to work on the original `challenge.bin` and my `.syn9k` format files.

```csharp
for (int i = 0; i <= _mainMemory.GetUpperBound(0); i++)
{
    bw.Write(_mainMemory[i]);
}
bw.Write(MARKER); // memory done

for (int i = 0; i <= _registers.GetUpperBound(0); i++)
{
    bw.Write(_registers[i]);
}
bw.Write(MARKER); // registers done
```

Now that I have a functional save/load function, back to the adventure. 

Following the adventure path, I came to some dark caves. Down one passage was darkness and a Grue that will certainly eat me. Fortunately there is a ladder. Unfortunately... 
```
== Twisty passages ==
You are in a twisty maze of little passages, all alike.
```
Are they though?  I took a look around to see what I could find.

> You are in a maze of little twisty passages, all alike.

> You are in a little maze of twisty passages, all alike.

> You are in a twisty alike of little passages, all maze.

I ran some tests and found that the text wasn't random. Methodically exploring and tracking which "twisty maze of alike passages, all little" I arrived in, I was able to come up with this [map](Extras/CaveMap.jpeg).

All that exploration awarded me another code. 
```
Chiseled on the wall of one of the passageways, you see:

    81cecc9fcbfba60ff4883f0584217622

You take note of this and keep walking.
```

Escaping the maze and beating back the darkness, I continued my journey onward.

### Code 6 - Toss a Coin to Your Monument 
The next location in the adventure game are some ruins. There are five coins scattered around the location, each with a different number or symbol. In the main room is a locked door and a strange monument. 

```
There is a strange monument in the center of the hall with circular slots and unusual symbols.  It reads:

_ + _ * _^2 + _^3 - _ = 399
```

The puzzle is to fit the correct coins in the correct slots. For this puzzle I used a spreadsheet.

[Five minutes later](Extras/CoinPuzzle.png).

The north door opened up and I continued with the adventure. Finding a teleporter, I used it and was whisked away.

```
You activate the teleporter!  As you spiral through time and space, you think you see a pattern in the stars...

    0855b015740ed8a7a72a2568ed0c6a97
```
Six codes collected!

### Code 7 - The Tale of Two Codes
#### Part 1 - We're not in Zork Anymore.  
The teleporter deposited me in the lobby of Synacor Headquarters. Exploring the area, I found an book about teleportation.
```
The cover of this book subtly swirls with colors.  It is titled "A Brief Introduction to Interdimensional Physics". 
```
The text details the next puzzle. In order to leave, the teleporter needs to be reprogrammed. The eighth register needs to be set to a specific value by extracting the validation routine and re-implementing it on more powerful hardware.

That made my next task clear. I needed to write a debugger. This triggered the next big refactor of my code.

What is useful in a debugger? I made a feature list of things I thought I might need. 
- Set memory values, including `instPtr`.
- Step through the code.
- Breakpoints.
- Dump the current state to a file and / or disassemble a binary.
- Some way to control everything and get useful output.

That's a bunch.

The first concern is control. With this I'd be adding a lot more commands to my twinned command switch statements. As there are two of them, one in the shell and one in the program, I'd potentially need to teach both of them about the upcoming debug commands. 

I'm allergic to duplicated work, so I stopped and had a think. What I needed is a single place to control input and output. This task fell to the outer shell.

The command loop would now be tasked to read in a line of input, then decide if it's a shell command, a debug command or a game command.

I removed all console input and output and worked an output buffer into `Synacore9000()`, making it I/O agnostic. I chose to implement the output buffer as a queue. A `List()` *should* return text in the order it is added. A `Queue()` *guarantees* the order and comes with the bonus of built in removal.

```csharp
case 19:         // out: 19 a    write the character represented by ascii code <a> to the terminal
    char c = (char)Ptr_ReadValue();

    _sbOutput.Append(c);
    if (c == '\n')
    {
        _outputBuffer.Enqueue(_sbOutput.ToString()); 
        _sbOutput.Clear();
    } 
    break;
case 20:        // in: 20 a     read a character from the terminal and write its ascii code to <a>;
    if (string.IsNullOrEmpty(_inputBuffer))
    {
        _instPtr--; // back this up to what should be the instruction, so if/when we resume we don't freak out.
        return State.Paused_For_Input;
    }

    ushort value = _inputBuffer[0];
    ushort address = Ptr_ReadRaw();

    _inputBuffer = _inputBuffer.Length > 1 ? _inputBuffer[1..] : string.Empty;

    Mem_Write(address, value);
    break;
```

This simplified the two op-codes and gave me a new problem to solve before starting on the debugger; program flow control between the class and the host program.

I enumerated the two states the program could be in, `State.Running` and `State.Halted`. Then tweaked the `Dispatcher()` and `Run()` methods appropriately. This replaced the global variable that was controlling the run loop.

It turned out I needed one more state, `State.Paused_For_Input`, so I could tell if the program is stopped because it's done, or stopped because its input buffer is empty.

```csharp
public State Run()
{
    State currentState;
    do
    {
        currentState = Dispatcher(Ptr_ReadValue());
    } while (currentState == State.Running);

    return currentState;
}
```
To differentiate between shell and/or debug commands with game commands, I pre-pended the shell and debug commands with `!`.

For example:
> `drop table` is a game command to unload some inventory.

> `!drop table` is sent to the command processor and could make for a bad day in production.

In rough psudocode, this is the outer loop flow.
```
while shell is running 
    read the input.

    if the input starts with '!'
        send it to the command processor
    else
        set the Synacor9000's input buffer
        Synacor9000.Run()
        write the output buffer to the console 
    end if-else
loop 

command processor 
    if the command is a shell command (shell commands are: load, run, save, exit) 
        execute it.
    else 
        Passes the command to the Synacore9000 debugger.
    end if-else
```

Now that I've sorted out my I/O, on to the debugger.

The method `DebugDispatcher()` was created in `Synacor9000()` and it quickly blossomed into an enormous set of nested switch statements. There were not many problems implementing this. I had my framework and I/O figured out. I did need to create yet another run loop for the step command, however, everything seemed to go smoothly.

I turned on tracing and started loaded the binary. My trace log exploded. I later discovered that writing a single character to the screen is in the neighborhood of thirty eight instructions. On the first game screen there are over 250 characters.

38 instructions... 250+ characters... It's over ***9000!!!*** And that's before game logic or other tasks. 

To preserve sanity and hard-drive space, I added the ability to toggle the trace on and off and set limits on the trace buffer. I considered adding exclusion zones, however, I didn't want to inadvertently exclude sections that might be relevant. Instead I'd need to be picky with what I logged. 

As a final bit of preparation, I decompiled the binary. 
```
...
2013  set  reg[2]       1        
2016  set  reg[5]       0        
2019   jf  reg[0]    2092        
2022   eq  reg[4]  reg[2]   10000
2026  set  reg[3]  reg[0]        
2029   jt  reg[4]    2040        
2032 mult  reg[1]  reg[2]      10
2036  mod  reg[3]  reg[0]  reg[1]
2040  set  reg[4]       0        
2043 mult  reg[2]  reg[2]   32767
...
```

With everything in place, it was time to go and find the fabled eighth register. `CTRL-F` is our friend. 
```
Find all "reg[7]", Current document
\Full_Dump.txt(270):521   jt  reg[7]    1093
\Full_Dump.txt(2500):5451   jf  reg[7]    5605
\Full_Dump.txt(2531):5522  set  reg[0]  reg[7]
\Full_Dump.txt(2737):6042  set  reg[1]  reg[7]
```

`521` is in the middle of the self test code. I'll ignore that one for now.

```
5445 push  reg[0]                
5447 push  reg[1]                
5449 push  reg[2]                
5451   jf  reg[7]    5605	
```

Fiddling with breakpoints, I found that this is called after the "use teleporter" command is issued. 

The eighth register is initialized to zero, so following the jump to `5605` is likely the path that leads to Synacore headquarters.

I have the tools, now time to put them to use.

```
> !load teleporter.syn9k
*** : Load of teleporter.syn9k done.
> !break addy 5451
Added address breakpoint 5451
> !set register 7 1
register: 7 set to: 1
> !set input use teleporter
inputBuffer: set to: use teleporter
> !break run
Program entered Paused_For_Input
> !trace echo
Command trace console echo is now on.
> !step 10
    5451   jf 32775  5605       :   2708  5445     3    10   101     0     0     1
    5454 push 32768             :   2708  5445     3    10   101     0     0     1
    5456 push 32769             :   2708  5445     3    10   101     0     0     1
    5458 push 32770             :   2708  5445     3    10   101     0     0     1
    5460  set 32768 28844       :   2708  5445     3    10   101     0     0     1
    5463  set 32769  1531       :  28844  5445     3    10   101     0     0     1
    5466  add 32770  3834 26343 :  28844  1531     3    10   101     0     0     1
    5470 call  1458             :  28844  1531 30177    10   101     0     0     1
    1458 push 32768             :  28844  1531 30177    10   101     0     0     1
    1460 push 32771             :  28844  1531 30177    10   101     0     0     1
```
Nice! 

I spent some time getting to know `1458`. That is text generation function that blew my trace out of the water. I can ignore it. I certainly don't need to step through it. Adjusting the breakpoints, I can see what the system wrote.  

```
> !break addy 5483
Added address breakpoint 5483
> !break run
Break on address 5483
PROGRAM OUT: A strange, electronic voice is projected into your mind:
PROGRAM OUT:
PROGRAM OUT:   "Unusual setting detected!  Starting confirmation process!  Estimated time to completion: 1 billion years."
PROGRAM OUT:
```

Turning back to the full dump, after the text output there is a block of noop codes. The next interesting set of instructions start at `5483`.

```
5483  set  reg[0]       4
5486  set  reg[1]       1
5489 call    6027
5491   eq  reg[1]  reg[0]       6
5495   jf  reg[1]    5579
```

`6027` is probably the validation function. We don't need that. So... 

```
> !set memory 5489 21
address: 5489 set to: 21
> !set memory 5490 21
address: 5490 set to: 21
> !break run
Program entered Paused_For_Input
PROGRAM OUT: A strange, electronic voice is projected into your mind:
PROGRAM OUT:
PROGRAM OUT:   "Miscalibration detected!  Aborting teleportation!"
PROGRAM OUT:
PROGRAM OUT: Nothing else seems to happen.
PROGRAM OUT:
PROGRAM OUT:
PROGRAM OUT: What do you do?
```
That's not what I want.

`5491` is looking for the value of register 1 to be 6. This must be the return value from the validation function. Let's back up and fix that.  

```
> !set memory 5494 4
address: 5494 set to: 4
> !break run
Program entered Paused_For_Input
PROGRAM OUT: You wake up on a sandy beach with a slight headache.  The last thing you remember is activating that teleporter... but now you can't find it anywhere in your pack.  Someone seems to have drawn a message in the sand here:
PROGRAM OUT:
PROGRAM OUT:     plzAsKTPdyqQ
```

Success! At this point it was about 1 a.m. I happily jotted down my newest code and went to bed.

```
"You didn't think it would be that easy, did you?"
"You know, for a second there, yea, I kinda did."
-O-Ren Ishii & The Bride, Kill Bill Vol. 1
```

At 4 a.m. my brain woke me up with a nagging feeling that the solution was too easy. The posts I'd read talked about the seventh code being the apex of the challenge.

At 6 a.m. I gave up on sleep.

plzAsKTPdyqQ => "plz AsK TP ?die? qQ". There is no way this is the real code.  

#### Part 2 - Hackerman? No, the other guy.

It's time to come to grips with reading assembly. After staring at this for a few hours, I had a basic idea of how it worked, even if I didn't quite get what it was doing.

`6027` is the entry point. `reg[0]` and `reg[1]` seem to act as parameters.
`jump` instructions are flow control.
`call` and `ret` instructions appear to define function entry and exit points.

```
5483  set  reg[0]       4
5486  set  reg[1]       1
5489 call    6027
5491   eq  reg[1]  reg[0]       6
5495   jf  reg[1]    5579
```
From the `eq` test in `5491` I can infer that `reg[0]` will contain the return value of `Func_6027(4, 1)`.

So what does `6027` do?
```
6027   jt  reg[0]    6035
6030  add  reg[0]  reg[1]
6034  ret                        
```
I reworked this into a new C# function. 

```csharp
ushort Func_6027(ushort a, ushort b)
{
    if (a != 0) return Func_6035(a, b);
    return b + 1;
}
```
That seems short and to the point. What's next?

```
6035   jt  reg[1]    6048
6038  add  reg[0]  reg[0]   32767
6042  set  reg[1]  reg[7]
6045 call    6027
6047  ret 
```
Ok, so this is odd. What's happening with the instruction at `6038`? 

Simply adding 32767 to a number will put the number out of bounds. However, checking the spec and implementation for `add`, the operation is modulo 37678.

```csharp
value = (ushort)((b + c) % MODULO)
```
The modulo makes this a subtraction operation. Cool.

```csharp
ushort Func_6035(ushort a, ushort b)
{
    if (b != 0) return Func_6048(a, b);
    a = a - 1;
    b = reg[7];
    return Func_6027(a, b);
}
```
One more to go.

```
6048 push  reg[0]                
6050  add  reg[1]  reg[1]   32767
6054 call    6027                       
6056  set  reg[1]  reg[0]        
6059  pop  reg[0]                
6061  add  reg[0]  reg[0]   32767
6065 call    6027                
6067  ret                        
```

This one involves the stack. In other code I've seen the stack is used to maintain state between function calls.

In order:
- save the value of reg[0] to the stack
- subtract 1 from reg[1]
- reg[0] = Func_6027(a, b)

The next instruction copies reg[0] to reg[1] then pops the stack contents back to reg[0];

- reg[0] = a (from the stack)
- reg[1] = has the result of our function call.
- and then we call... Func_6027(a, b);

```csharp
ushort Func_6048(ushort a, ushort b)
{
    return Func_6027(a, Func_6027(a, b - 1));
}
```
Oh dear me.

```csharp
ushort RecursiveNightmare(ushort r0, ushort r1)
{
    if (r0 == 0) return r1 + 1;
    if (r1 == 0) return RecursiveNightmare(r0 - 1, reg[7]);
    return RecursiveNightmare(r0, RecursiveNightmare(r0, r1 - 1));
}
```

How the [REDACTED] do you optimize *that?* 

I learned later that this is called [Ackermann's function](https://en.wikipedia.org/wiki/Ackermann_function). For now, it's pure horror.

I made a new class called `PuzzleSolutions()` and dumped `RecursiveNightmare()` into it.

Giving it a test run, it immediately blew through the stack. I needed something to bring down the recursion. The easiest thing to add is a cache for the return values.
```csharp
if (memo.TryGetValue((r0, r1), out ushort memoReg)) return memoReg;

// recursion nightmare logic goes here 

memo.TryAdd((r0, r1), ret);
return memo[(r0, r1)];
```

The stack lasted longer, but still blew up. C# has a 1 MB stack for 32-bit programs and a 4 MB stack for 64-bit programs. My next, possibly predictable, question was: How can I increase the stack size? Everything I found online said some variation of "This is a bad idea. You're an idiot. Fix your program. Don't fkin' touch it you id10t n00b."

And then there was this: 
```
Thread(ParameterizedThreadStart, Int32)	
Initializes a new instance of the Thread class, specifying a delegate that allows an object to be passed to the thread when the thread is started and specifying the maximum stack size for the thread.
```

Jackpot.

```csharp
Thread tp = new(new ParameterizedThreadStart(solveTP.TeleporterSolver), 18000000);
```

The *MINIMUM* stack that allowed this thread to run was 18 MB. Over four times the default for a 64 bit application.

But it ran.

... and it didn't return a valid answer.

***frustrated programmer noises***

I tore that function apart. I checked it line by line against the source assembly. I stepped through it. It seemed to work. I took the step of looking up another solution and comparing my code to it. They were functionally identical.

In hindsight, this should have been a pretty big clue that the problem wasn't with the function.

Literally hours later I found it. The problem was with the helper functions that did the modulo math.
```
static ushort SubOne(ushort a) => (ushort)((a + USHORT_32767) % MODULO);
static ushort AddOne(ushort a) => (ushort)((a + USHORT_1) % MODULO);
```
Everything looks ok here... 
```
const ushort USHORT_32767 = 32757;
const ushort MODULO = 32758;
```
Wait a second.

*OH! BY THE [OMNISSIAH's](https://warhammer40k.fandom.com/wiki/Machine_God) BENT SPARK PLUGS!* 

```
> !load teleporter.syn9k
*** : Load of teleporter.syn9k done.
> !solve_teleporter
*** : The teleporter solution code found: [teleporter code]
> !set register 7 {teleporter code]
register: 7 set to: [teleporter code] 
> !set addy 5485 6
address: 5485 set to: 6
> !set addy 5489 21
address: 5489 set to: 21
> !set addy 5490 21
address: 5490 set to: 21
> !run
*** : Running the program.
> use teleporter
A strange, electronic voice is projected into your mind:

  "Unusual setting detected!  Starting confirmation process!  Estimated time to completion: 1 billion years."

You wake up on a sandy beach with a slight headache.  The last thing you remember is activating that teleporter... but now you can't find it anywhere in your pack.  Someone seems to have drawn a message in the sand here:

    869555826e04d6aea29850b2680c9b01
```

Seven of eight. Almost there.

Before I moved on to the last stage of the puzzle, I knew I had to do something about that method. I did not like `Thread()`. I wasn't sure how to get data back from it. The output was written direct to the console. 

`Task()` seemed to be more in line with spinning off batches of parallel tasks to search for the solution. However, there wasn't any stack control with a `Task()`. Without a stack increase, I'd have to truly optimize the algorithm. During my debugging and code diving, I found a couple of versions of the algorithm that were not recursive and would run without environment changes. 

I implemented one from a [JavaScript solution.](https://github.com/NiXXeD/synacor-challenge/blob/master/spoilers/teleporter.js) While the routine is obvious to me in hindsight, I don't know if I'd have been able to arrive at that solution on my own. My [implementation](PuzzleSolutions.cs) (`PAckAsync()`) is heavily commented for any that are interested.

As a final note, in this scenario not using `Task()` is faster than using it. Regardless, I gained the experience of implementing an `async` method, so I count it as win.

### Code 8 - A Tropical Vacation

Through the teleporter and out the other side to a brand new location. Time to go exploring. Exploring the tropical island I found a journal that talked about a locked vault and a mysterious orb that would gain or lose weight as it was moved from room to room. There is mention of a hourglass, so it's likely that we need to move quickly, possibly in the fewest number of moves. 

Orb puzzle consisted of a simple 4x4 grid. Each room had a number or a symbol associated with it. Inspecting the orb reveled the number 22. The vault door had the number 30 carved into it. 

One quick map later... 
```
[ * ] - [ 8 ] - [ - ] - [ 1 ] -> Vault 30
  |       |       |       |
[ 4 ] - [ * ] - [ 11] - [ * ]
  |       |       |       |
[ + ] - [ 4 ] - [ - ] - [ 18]
  ^       |       |       |
[ 22]-> [ - ] - [ 9 ] - [ * ]
  |
Start
```

The puzzle seemed clear. Move the orb from start to finish in the fewest steps, arriving with a weight of 30.

Some experimentation revealed additional aspects to the puzzle.
1) You cannot return to the first room. The orb resets. 
2) You cannot leave the vault room. The orb vanishes on arrival. 
3) The orb will vanish if its weight becomes negative.

Having done two years of Advent of Code puzzles, I had seen variations of this one before. I dusted off a breadth first search. The main change I made was to have each "step" in the puzzle cover two squares, an operator and a number. I added one additional bounds check to ensure the weight never exceeded 100. This was to prevent loops from forming. The program chewed away at the problem, then spat out an answer.

Almost predictably, it was wrong. 

It didn't take me long to figure out my oversight. I'd setup my steps to be up/down then left/right and vice versa. I did not account for double up/down or double left/right.

One quick correction later, and volia!

```
== Vault ==
This vault contains incredible riches!  Piles of gold and platinum coins surround you, and the walls are adorned with topazes, rubies, sapphires, emeralds, opals, dilithium crystals, elerium-115, and unobtainium.

Things of interest here:
- mirror
```

The mirror is *obviously* the most interesting item.

```
Through the mirror, you see "d8f1e2a126c21ae6791cea6a335784c8" scrawled in charcoal on your forehead.

Congratulations; you have reached the end of the challenge!
```

And just like that ... we're done.

### Conclusion
Final Thoughts. 