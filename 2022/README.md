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
- Testing ranges of numbers on an integer grid
- Simple seemed best. There is probably a solution that involves slicing data that is compact, quick and wrong.

Day 9 - Rope Bridge
- Snake simulator.
- In this one we iteratively  apply some simple rules to an array of Points, shifting them one at a time to follow the leader.

Day 10 - Cathode-Ray Tube
- Simple register and clock simulation
- My answer to this was to simulate the two instructions, with the addx instruction morphing between cycles to indicate if it was on its load or execute sequence. 

Day 11 - Monkey in the Middle
- This is a game of moving items around, with a growing "worry factor" that needs to be tracked and operated on for each move. 
- Part 2 of this puzzle involved Chinese Remainder Theorem as a way to manage the super large numbers the "worry factor" would become.

Day 12 - Hill Climbing Algorithm
- Find the shortest path from A to B. 
- Implemented an A* pathfinding algorithm based off the Wikipedia psudocode. 

Day 13 - Distress Signal
- Comparing nested lists of integers and lists, which can be blank. 
- Packet object implements IComparable, IEquatable, and IComparisonOperators all so I can use A < B and List<Packet>.Sort();

Day 14 - Regolith Reservoir
- Tetris! 
- The key here of measuring height after X rocks is to figure out when the pattern repeats, and then math it out.

Day 15 - Beacon Exclusion Zone
- Find a spot not covered by the area of other rectangles. 
- Implemented a solution where I walked the perimeter of each rectangle.

Day 16 - Proboscidea Volcanium
- Traveling Salesman type problem. 
- After struggling with Branch and Bound, I wound up implementing a very neat solution from reddit user u/juanplopes.

Day 17 - Pyroclastic Flow
- Tetris! 
- The key here of measuring height after X rocks is to figure out when the pattern repeats, and then math it out.

Day 18 - Boiling Boulders
- Given a 3d object defined by 1x1x1 cubes, find the area of it, and identify the area of pocket with it. 
- Brought back A* to find if an area had a path from inside to outside. This required working out the rules for a 3D point that worked with IComparable to be used in the algorithm.

Day 19 - Not Enough Minerals
- Optimization of robot tasks.
- Depth first search. RoboState does the heavy lifting of advancing time and holding the current state of the factory. RoboBlueprint serves as the primary container for each blueprint string.
- A key learning item was the formula ( x + (y-1)/y ) to find the celing in integer division. 

Day 20 - Grove Positioning System
- Circular double linked list manipulation.
- A straight forward routine to move an item left or right in the list by calculating it's new position (accounting for loops) and updating the links to/from it

Day 21 - Monkey Math
- Binary tree walking
- Working down the tree for Part 1 is simple recursion.
- Working UP the tree for Part 2 involved some... trickery. Part of the complexity is the math can change if you're coming up from the right or left of an element, so there's constant checks against the parent. 

Day 22 - Monkey Map
- Pathfinding and cube folding.
- Split the map up into six indivual "sides". Each side contians a link to its neighbors. As you walk the map, when you step off one, use the link information to place yourself on the new map.
- For part two, we add a translation function when transitioning form map to map to account for differences in orientation.
- There are 11 nets for an unfolded cube. At the moment, the "folding" is hard coded. A future TODO would be to perform a more generalized linking.

Day 23 - Unstable Diffusion
- Cell automata problem.
- Farily straight forward solution. Currently needs a performance pass, as the Where/FindAll test for neighbors is super slow.
- Speedups were achieved by dropping the Elf object in favor of a HashSet<Point> for the elves combined with generating a neighbor set to search across. 

Day 24 - Blizzard Basin
- Pathfinding through a shifting map
- I modified the A* pathfinding routine from day 12 to include a 3D point where the Z was used as a time dimension.
- For each step I calculated backwards to determine where a storm would have to start to intersect the currnet position, then checked the inital map for that information.

Day 25 - Full of Hot Air
- Number conversion puzzle with the twist that some digits can represent negative numbers. 
- Adapted a general to/from base 10 routine I'd developed a decade ago for another project.
- Extended the routine to be a general to/from conversion with the ability to load in custom character dictionaries. 
- Future fun: Make it a Base X to Base Y converter. IE: Hex to Octal w/o the need to go Hex -> Dec -> Oct