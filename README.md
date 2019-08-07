# Anaximander.Linq
*Useful LINQ extension methods and utility classes for digging deeper into data.*

[![NuGet Badge](https://buildstats.info/nuget/Anaximander.Linq)](https://www.nuget.org/packages/Anaximander.Linq/)

This is a library of various methods and classes for inspecting, manipulating and projecting collections of objects in C#, assembled from a variety of things that I've found useful along the way. The package is intended to cover uses cases including combinatorics, statistics, and data analysis. 

## Features

### BoxWhile

Divides a collection into "boxes" while a condition holds true between successive elements. For example:

    var sessions = userActions.BoxWhile((a, b) => a.Timestamp - b.Timestamp < Timespan.FromMinutes(30));

The above code will divide user actions into groups where each action in a group is less than thirty minutes after the one preceding it. This could be a convenient way to identify contiguous login sessions.

### CartesianProduct

Generates the [Cartesian product](https://en.wikipedia.org/wiki/Cartesian_product) of two or more sets. This is useful when working with data that is to be combined or compared.

### Combinations

Generates all combinations of a certain size from a set of source items. For example:

    var pairs = items.Combinations(2, CombinationsGenerationMode.DistinctOrderInsensitive);

The above code will find all the ways that the source set can be arranged into pairs. Note the `CombinationsGenerationMode` parameter; this allows the user to specify whether each source item can be used only once, or multiple times, and whether a re-ordering of the same elements is considered a new combination. For example, with a source collection of `{ 1, 2, 3 }`, in `Distinct*` mode, the pair `{ 1, 1 }` is not valid, while in `AllowDuplicates*` mode this combination is allowed. In `*OrderSensitive` mode, `{ 1, 2 }` is considered to be a different combination to `{ 2, 1 }`; in `*OrderInsensitive` mode they are considered to be the same. 

### Minima and Maxima

This set of methods allow the searching of ordered data for elements that are local minima or maxima. In other words, it allows the location of peaks and troughs in data. These methods offer variants that return the value, or the index within the source collection, as well as providing overloads that let the calling code specify how two items should be compared.

### OrderToMatch

This method allows the ordering of one set based on the order of another. The user provides a set of "key" values, and a way to derive a key from the objects to be ordered. Items whose key does not appear in the provided key set are sorted to the end.

### Permute

This method generates all permutations of a set - that is, all ways of putting that set into a different order. Note that the output of this operation scales with the factorial of the set size; this causes a very serious performance impact as the input set becomes large. For example, a set of 5 elements can be ordered 120 ways; a set of 10 elements can be ordered 3,628,800 ways. A set of 20 elements has 2,432,902,008,176,640,000 permutations, which is too many to count with a 64-bit integer. Use this method with caution.

### Shuffle

This method returns an enumerable that will randomly re-order its contents every time it is enumerated, which can be useful for randomising samples or testing sort algorithms.

### Slices and SlicesOf

These methods slice a collection into equally-sized portions. `SlicesOf()` creates slices of a user-specified size, while `Slices()` lets the user specify how many slices there should be, and makes the slices the appropriate size. The original order of the items is maintained - for example, slicing the numbers from 1 to 10 into threes would produce `{ 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10 }`. The sliced enumerable can be enumerated directly, in which case all slices are returned in order, and the remainder is returned as the last; alternatively, the `Slices` property allows the calling code to access only the evenly-sized portions, and the `Remainder` property contains the leftover portion, if the collection could not be evenly divided.

### Window

This method provides a moving window over a collection. For example:

    var windows = Enumerable.Range(1, 10).Window(3);

The above code would return `{ 1, 2, 3 }`, then `{ 2, 3, 4}`, then `{3, 4, 5}` etc. until `{ 8, 9, 10 }`. This can be useful for buffering, or calculating moving averages.
