# Synacor Challenge

## Developed by [Eric Wastl](https://github.com/topaz). 
## NOTE: As of the time of this commit, the [original site](https://challenge.synacor.com/) appears to have been taken offline. Reddit user [/u/Aneurysm9](https://www.reddit.com/r/adventofcode/comments/11pjsxk/comment/jbzkpo3/) has posted a [repository](https://github.com/Aneurysm9/vm_challenge) with the puzzle binary, documentation and hashes of their solution codes.

### Introduction
In the words of the former website: 

>This challenge was given by Synacor at php[tek] and OSCON in years past. Now, we are opening it up to everyone!  
This is a programming challenge, but you can use any language you like. As you complete sections, come back here to check your answer codes.  
The winner during each convention got Apple gift cards and fame. While we're not giving away physical prizes any more, we still hope you'll find the challenges fun and interesting!

In the December of 2022, I learned about [Advent of Code](https://adventofcode.com/) (AoC) coding puzzles from a co-worker. That fortuitous mention reignited my enjoyment of coding for fun. I've been maintainting a legacy system, so these puzzles doubled as a way to modernize my skill set. I choose to learn C# as it aligns with my professional goals.

During my brief time with the AoC community I learned of the Synacor Challenge. Having finished a block of AoC puzzles, I decided to take a break and spend some time seeing what I could do with the Synacor Challenge.

The overall setup is straightforward: Given a personalized binary and an architecture sheet, extract the eight codes from the puzzle and submit them to the website.

I've had some minor experience working with binaries, but  I've never actually done anything like this before.

Since completing the challenge, I have read the writeups for several solutions. I found they tended to narrowly focus on the puzzles, and not what went into building the VM required to solve the puzzles. Below is my journey, which I hope will be of interest of anybody who started this puzzle with an experience level similar to my own.

**Everything after this is filled with spoilers.**

One final note. All the challenge codes sown below are hashed with MD5. If you take the challenge with my binary, you can compare the hashes of the codes that you've gotten to the codes I've gotten to see if you're on the right track.

Here we go! 

### Code 1 - RTFM!
There are two files provided, `challenge.bin` and `arch-spec`. Leaving the bin file alone, I opened up the arch-spec and read it over. It provided definitions for the memory, the numbers, some system limits, a few hints, and a listing of the op-codes. The only thing that gave me pause was the comment about the numbers being stored in a little-endian format. I had visions of endless endian bugs. Fortunatly, this format lines up with my development tools and hardware. 
Creating a new C# console project, I read through the specification again, then started laying out some basics as I felt out what I would need to implement the program.[^1]

The specification states all numbers are unsigned 15 bit integers. I set aside `int` and welcomed my new best friend, `ushort`. `ushort` is a 16 bit number, thus it can contain larger values then the specification permits.

Registers are defined as being addressed above main memory.

So let's define some limits so we can refer to them later.
```csharp
const ushort MAX_VALUE = 36768;
const ushort ABSOLUTE_MAX = 36776;
```

Main memory and registers map neatly to arrays. The stack is a `Stack()`. I could see no reason to reinvent the ~~wheel~~ `Stack()`.

I considered merging main memory and the registers into one large array. The addressing seemed to support that idea. I ultimately rejected that option as setting up potential trouble down the line. The spec dictated three memory regions, so I implemented three regions, not two. [Five is right out.](https://montypython.fandom.com/wiki/Holy_Hand_Grenade_of_Antioch)
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

### Code 2 - Implement the basics. 
My next task was to load the `challenge.bin` file into 'mainMemory[]'. With a general lack of experience working with binary files, I immediatly turned to the help documentation. Pilfering some example code, I loaded the binary into `mainMemory[]`.

Almost.

I briefly tripped over the subtle but critical detail that the `BaseStream.Position` doesn't directly map to the the indexes for `mainMemory[]`. 

After adjusting to account for the offset, the binary successfully loaded into `mainMemory[]`.
```csharp
using BinaryReader r = new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read));
while (r.BaseStream.Position < r.BaseStream.Length)
{
    mainMemory[r.BaseStream.Position / 2] = r.ReadUInt16();
}
```

I still didn't know what I didn't know, so I focused on the arch-spec suggestion of starting with getting op-codes 0, 19 and 21 working.

Checking the op-code list:
- 0 is the "halt" command. 
- 19 outputs a character.
- 21 is the "no operation" command.

Op-code 21 was an oddly important code to implement. Support for it required the basic framework of read instruction -> process instruction -> repeat.

With that in mind, the immediate goal was to finish off the framework. First up was the main loop to read the instructions from memory.[^2] The next task was to build a function that does the real work.

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
... and so on for all 21 codes.

There were a couple of things that I knew would need to be addressed.
1) By passing `cursor` as a parameter and then returning it to the run loop, it made `DoInstruction()` responsible for maintaining the state of `cursor`. A better idea would be to pass the instruction directly to `DoInstruction()`. This does not solve the problems of knowing how far to advance the cursor and how best to communicate the parameters.
2) I have an allergy to magic numbers, so all of those `nextInstruction = (ushort)(cursor + 2);` lines would have to be addressed.
3) I didn't like the need to continually cast to `ushort` for basic math. After some digging, I discovered that `cursor + 2` is `int` math. In this example, `cursor` was cast to be an `int`. 2 was an `int` by default and `+` does `int` math returning an `int`. This will be something to watch out for.

Still, despite the early roughness of the code, the program loads, runs, and presented this message.

```
Welcome to the Synacor Challenge!
Please record your progress by putting codes like
this one into the challenge website: 7acb6bd7646fa25e711e53f1b4d385f7

Executing self-test...

jmp fails
```
A second code! Things were certainly progressing well.

### Code 3 - Implement the rest of the op-codes. 
This was my first big refactor. I split out nearly all the code written so far into its own object: `Synacor9000()`.

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


I implmented a set of helper methods. `Mem_Write()` writes a value to a memory or register address. `Mem_Read()` returns the value from a memory or register address.

The biggest helpers were `Ptr_ReadValue()` and `Ptr_ReadRaw()`.
`Ptr_ReadValue()` returns the value of the current memory address, or the value of a register. `Ptr_ReadRaw()` always returns the value.

For example, if `Ptr_ReadValue()` reads 2134, it will return 2134.  However, if it reads 32770, it will return the contents of register 3.

For the same example, `Ptr_ReadRaw()` would return 32770.

I made an architectural decision of my own. When the `Ptr` methods were called they would advance `instPtr` by one. This simplified my code immensely. I didn't need to know anything about any particular op-code's requirements. The act of an op-code requesting data would automatically set the pointer to the proper location for the next request.

I took the opportunity to rename some functions and variables. For example, `DoCommands()` became `Dispatcher()` and `cursor` became `istrPtr`. I added `Run()` and `Load()` methods. `Dispatcher()` lost the `param` parameter, now accepting only the current instruction.  

I broke the op-codes into smaller methods of realated functionaly. Each block shared the same basic format, similar to the final form of the one below.
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

Op-code implementation went smoothly, each implemented code passed the presented self check until I was left with only three unimplemented op-cdes. Spirits were high.

This is where I slammed into my first roadblock.

#### The Tale of rmem and wmem 
> rmem: 15 a b	    read memory at address \<b> and write it to \<a>


> wmem: 16 a b	    write the value from \<b> into memory at address \<a>


At first glance they seem to be the same code. Data goes from \<b> to \<a>. The devil, as the saying goes, is in the details.

Two things conspired to keep me from a solution. 

First, and most importantly, I didn't fully understand the instructions. After flailing for some time, I sought out some help and found a reddit post that nudged me towards a solution.

For `rmem` the target is a raw value from memory. Like many other op-codes,  
`address = Ptr_ReadRaw();` reads the correct value.

The source is a whole other animal. "Read memory at address \<b>" This is a two step redirection. We want the value of the memory of the address stored in \<b>. 
In code terms: `value = Mem_Read(Ptr_ReadValue());`

Once I understood that, it exposed my second issue. At the time, `Ptr_ReadValue()` called `Mem_Read()` directly: `Mem_Read(instPtr++)`. This would give me the value at the address stored in `instPtr`. This wasn't wrong for much of my code. 

`Mem_Read()` would read the value from any address passed to it. `Ptr_ReadValue()` would return the value in `mainMemory[]`, unless that value mapped to a register. 

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

During my snooping for answers regarding `wmem` I unfortuanly caught a hint of how to handle this. "Input buffer."

C# has three ways to read `Console` input. `Console.Read()`, `Console.ReadKey()`, and `Console.ReadLine()`. These will read a `char`, a keypress and a `string` respectivly. I ran a few experiments with them and settled on using `ReadKey()`





```
What do you do?
> use tablet

You find yourself writing "3f385e97cb9dc86ed1cd10f74184ba2b" on the tablet.  Perhaps it's some kind of code?

What do you do?
>
```
We go exploring, that's what we do!

### Code 5 - Cavern Code
```
== Twisty passages ==
You are in a twisty maze of little passages, all alike.
```

Are they though?  Let's look around a little and see what we can find. 

> You are in a maze of little twisty passages, all alike.

> You are in a little maze of twisty passages, all alike.

> You are in a twisty alike of little passages, all maze.

Some testing showed that linkages from room to room seemed stable, and the text isn't random. That's enough to uniquely identify each room. 

So I [mapped it](Extras/CaveMap.jpeg).


```
Chiseled on the wall of one of the passageways, you see:

    81cecc9fcbfba60ff4883f0584217622

You take note of this and keep walking.
```

That was fun. What puzzle comes next? 

### Code 6 - Ruins 


Coin Puzzle -> Teleporter.
0855b015740ed8a7a72a2568ed0c6a97

### Code 7 - Hacked teleport

Debugger -> New Teleport.
869555826e04d6aea29850b2680c9b01

### Code 8 - Maze and end game
Orb room.
d8f1e2a126c21ae6791cea6a335784c8

### Conclusion
Final Thoughts. 


[^1]: As I write this and look back through my commits, it's crazy how much things changed and grew.
[^2]: Oh gawd, that `param` array... I promise that it gets better soon.