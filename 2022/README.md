2022 Puzzle Summary 

Day 1 - Calorie Counting
- Load lists of integers, group, sum and sort them.
- Solution uses a pair of LINQ statements

Day 2 - Rock Paper Scissors
- Read input to determine the appropriate outcome in a RPS contest
- I use a pair of lookup tables and the input files as the indexes for the lookups.

Day 3 - Rucksack Reorganization
- Search the input for various string patterns. 
- There might be Regex for this, but I did it in a pair of straight forward for loops.

Day 4 - Camp Cleanup
- Determine if the ranges represented in pairs of numbers overlap. 
- Pull the numbers out of the input via Regex and pass through a pair of if statements for the solution.

Day 5 - Supply Stacks
- Move stacks of cargo between stacks. 
- Used an array of Stack objects to represent cargo and pop/pushed between them.
- Part2 involved moving multiple items at once, and keeping them in order. I used an extra stack in my array to maintain the order. 

Day 6 - Tuning Trouble
- Test substrings for uniqueness. 
- I used looped calls to Distinct() comparing the size of the return to the size of the string I was testing. 

Day 7 - No Space Left on Device
- Find free space in a directory hierarchy.
- In this one I worked to the strict requirements of the puzzle. I built a basic tree structure. When adding files to the tree, I added the size to all parent nodes.
- This allowed me to use LINQ to sort/query the nodes node and get the size for it and all children without any need to walk the tree. 

Day 8 - Treetop Tree House
- 

Day 9 - Rope Bridge

Day 10 - Cathode-Ray Tube

Day 11 - Monkey in the Middle

Day 12 - Hill Climbing Algorithm

Day 13 - Distress Signal

-Implements, IComparable, IEquatable, and IComparisonOperators
Day 14 - Regolith Reservoir

Day 15 - Beacon Exclusion Zone

Day 16 - Proboscidea Volcanium
-TODO

Day 17 - Pyroclastic Flow

Day 18 - Boiling Boulders

Day 19 - Not Enough Minerals
-TODO
Day 20 - Grove Positioning System

Day 21 - Monkey Math

Day 22 - Monkey Map
-TODO

Day 23 - Unstable Diffusion
-Cell automata problem.

Day 24 - Blizzard Basin
-TODO 

Day 25 - Full of Hot Air
- Number conversion puzzle with the twist that some digits can represent negative numbers. 
- Adapted a general to/from base 10 routine I'd developed a decade ago for another project.
- Extended the routine to be a general to/from conversion with the ability to load in custom character dictionaries. 
- Future fun: Make it a Base X to Base Y converter. IE: Hex to Octal w/o the need to go Hex -> Dec -> Oct 
