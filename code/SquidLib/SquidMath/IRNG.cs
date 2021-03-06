﻿using System.Collections.Generic;

namespace SquidLib.SquidMath {
    /**
     * Interface for full-featured random number generators to implement
     */
    public interface IRNG {
        /**
         * Get up to 32 bits (inclusive) of random output; the int this produces
         * will not require more than {@code bits} bits to represent.
         *
         * @param bits an int between 1 and 32, both inclusive
         * @return a random number that fits in the specified number of bits
         */
        int NextBits(int bits);

        /**
         *
         * Using this method, any algorithm that needs to efficiently generate more
         * than 32 bits of random data can interface with this randomness source.
         *
         * Get a random long between Long.MIN_VALUE and Long.MAX_VALUE (both inclusive).
         * @return a random long between Long.MIN_VALUE and Long.MAX_VALUE (both inclusive)
         */
        long NextLong();
        ulong NextULong();

        /**
         * Produces a copy of this RandomnessSource that, if next() and/or NextLong() are called on this object and the
         * copy, both will generate the same sequence of random numbers from the point copy() was called. This just needs to
         * copy the state so it isn't shared, usually, and produce a new value with the same exact state.
         * @return a copy of this RandomnessSource
         */
        IRNG Copy();

        /**
         * Get a random integer between Integer.MIN_VALUE to Integer.MAX_VALUE (both inclusive).
         *
         * @return a 32-bit random int.
         */
        int NextInt();

        uint NextUInt();

        /**
         * Returns a random non-negative integer below the given bound, or 0 if the bound is 0 or
         * negative.
         *
         * @param bound the upper bound (exclusive)
         * @return the found number
         */
        int NextInt(int bound);
        uint NextUInt(uint bound);

        /**
         * Returns a random long below the given bound, or 0 if the bound is 0 or
         * negative.
         *
         * @param bound the upper bound (exclusive)
         * @return the found number
         */
        long NextLong(long bound);

        ulong NextULong(ulong bound);

        /**
         * Get a random bit of state, interpreted as true or false with approximately equal likelihood.
         * @return a random boolean.
         */
        bool NextBoolean();

        /**
         * Gets a random double between 0.0 inclusive and 1.0 exclusive.
         * This returns a maximum of 0.9999999999999999 because that is the largest double value that is less than 1.0 .
         *
         * @return a double between 0.0 (inclusive) and 0.9999999999999999 (inclusive)
         */
        double NextDouble();
        /**
         * This returns a random double between 0.0 (inclusive) and outer (exclusive). The value for outer can be positive
         * or negative. Because of how math on doubles works, there are at most 2 to the 53 values this can return for any
         * given outer bound, and very large values for outer will not necessarily produce all numbers you might expect.
         *
         * @param outer the outer exclusive bound as a double; can be negative or positive
         * @return a double between 0.0 (inclusive) and outer (exclusive)
         */
        double NextDouble(double outer);

        /**
         * Gets a random float between 0.0f inclusive and 1.0f exclusive.
         * This returns a maximum of 0.99999994 because that is the largest float value that is less than 1.0f .
         *
         * @return a float between 0f (inclusive) and 0.99999994f (inclusive)
         */
        float NextFloat();
        /**
         * This returns a random float between 0.0f (inclusive) and outer (exclusive). The value for outer can be positive
         * or negative. Because of how math on floats works, there are at most 2 to the 24 values this can return for any
         * given outer bound, and very large values for outer will not necessarily produce all numbers you might expect.
         *
         * @param outer the outer exclusive bound as a float; can be negative or positive
         * @return a float between 0f (inclusive) and outer (exclusive)
         */
        float NextFloat(float outer);
        /**
         * Exclusive on bound (which may be positive or negative), with an inner bound of 0.
         * If bound is negative this returns a negative long; if bound is positive this returns a positive long. The bound
         * can even be 0, which will cause this to return 0L every time. This uses a biased technique to get numbers from
         * large ranges, but the amount of bias is incredibly small (expected to be under 1/1000 if enough random ranged
         * numbers are requested, which is about the same as an unbiased method that was also considered). It may have
         * noticeable bias if the generator's period is exhausted by only calls to this method. Unlike all unbiased methods,
         * this advances the state by an equivalent to exactly one call to {@link #NextLong()}, where rejection sampling
         * would sometimes advance by one call, but other times by arbitrarily many more.
         * @param bound the outer exclusive bound; can be positive or negative
         * @return a random long between 0 (inclusive) and bound (exclusive)
         */
        long NextSignedLong(long bound);

        /**
         * Returns a random non-negative integer between 0 (inclusive) and the given bound (exclusive),
         * or 0 if the bound is 0. The bound can be negative, which will produce 0 or a negative result.
         * <br>
         * Credit goes to Daniel Lemire, http://lemire.me/blog/2016/06/27/a-fast-alternative-to-the-modulo-reduction/
         *
         * @param bound the outer bound (exclusive), can be negative or positive
         * @return the found number
         */
        int NextSignedInt(int bound);
        /**
         * Returns a value between min (inclusive) and max (exclusive) as ints.
         * <br>
         * The inclusive and exclusive behavior is to match the behavior of the similar
         * method that deals with floating point values.
         * <br>
         * If {@code min} and {@code max} happen to be the same, {@code min} is returned
         * (breaking the exclusive behavior, but it's convenient to do so).
         *
         * @param min
         *            the minimum bound on the return value (inclusive)
         * @param max
         *            the maximum bound on the return value (exclusive)
         * @return the found value
         */
        int NextInt(int min, int max);

        /**
         * Returns a value between min (inclusive) and max (exclusive) as longs.
         * <br>
         * The inclusive and exclusive behavior is to match the behavior of the similar
         * method that deals with floating point values.
         * <br>
         * If {@code min} and {@code max} happen to be the same, {@code min} is returned
         * (breaking the exclusive behavior, but it's convenient to do so).
         *
         * @param min
         *            the minimum bound on the return value (inclusive)
         * @param max
         *            the maximum bound on the return value (exclusive)
         * @return the found value
         */
        long NextLong(long min, long max);

        /**
         * Returns a value from a uniform distribution from min (inclusive) to max
         * (exclusive).
         *
         * @param min the minimum bound on the return value (inclusive)
         * @param max the maximum bound on the return value (exclusive)
         * @return the found value
         */
        double NextDouble(double min, double max);

        /// <summary>
        /// Retrieves a random element from the given non-empty IList of T (or array of T).
        /// This runs in constant time if the element accessor in <see cref="IList{T}"/> runs in constant time,
        /// which is the case for <see cref="List{T}"/> and <see cref="Array"/> but not <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The generic type of IList</typeparam>
        /// <param name="list">An IList that must be non-null and non-empty; otherwise this returns default(T).</param>
        /// <returns>A random T element from list.</returns>
        T RandomElement<T>(IList<T> list);

        /// <summary>
        /// Retrieves a random element from the given non-empty IOrdered of T (or array of T).
        /// This typically should only be run on <see cref="IndexedSet{T}"/>; if you have an <see cref="IndexedDictionary{TKey, TValue}"/>,
        /// then use <see cref="RandomKey{TKey, TValue}(IndexedDictionary{TKey, TValue})"/> and/or <see cref="RandomValue{TKey, TValue}(IndexedDictionary{TKey, TValue})"/>.
        /// This should generally run in constant time.
        /// </summary>
        /// <typeparam name="T">The generic type of IOrdered</typeparam>
        /// <param name="ordered">An IOrdered that must be non-null and non-empty; otherwise this returns default(T).</param>
        /// <returns>A random T element from ordered.</returns>
        T RandomElement<T>(IOrdered<T> ordered);

        /// <summary>
        /// Retrieves a random element from the given non-empty ICollection of T (an IList or array uses a different overload).
        /// This runs at best in linear time, since it has to iterate through a random amount of coll before it has a result.
        /// </summary>
        /// <typeparam name="T">The generic type of ICollection</typeparam>
        /// <param name="coll">An ICollection that must be non-null and non-empty; otherwise this returns default(T).</param>
        /// <returns>A random T element from coll.</returns>
        T RandomElement<T>(ICollection<T> coll);

        /// <summary>
        /// Gets a random key from the given IndexedDictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the IndexedDictionary keys</typeparam>
        /// <typeparam name="TValue">The type of the IndexedDictionary values, ignored here</typeparam>
        /// <param name="dictionary">A non-null, non-empty IndexedDictionary</param>
        /// <returns>A randomly selected key from dictionary</returns>
        TKey RandomKey<TKey, TValue>(IndexedDictionary<TKey, TValue> dictionary);

        /// <summary>
        /// Gets a random value from the given IndexedDictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the IndexedDictionary keys, used internally</typeparam>
        /// <typeparam name="TValue">The type of the IndexedDictionary values</typeparam>
        /// <param name="dictionary">A non-null, non-empty IndexedDictionary</param>
        /// <returns>A randomly selected value from dictionary</returns>
        TValue RandomValue<TKey, TValue>(IndexedDictionary<TKey, TValue> dictionary);

        /**
         * Shuffle an array using the Fisher-Yates algorithm and returns a shuffled copy, freshly-allocated, without
         * modifying elements.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         *
         * @param elements an array of T; will not be modified
         * @param <T>      can be any non-primitive type.
         * @return a shuffled copy of elements
         */
        T[] Shuffle<T>(T[] elements);

        /**
         * Shuffles an array in-place using the Fisher-Yates algorithm.
         * If you don't want the array modified, use {@link #shuffle(Object[], Object[])}.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         *
         * @param elements an array of T; <b>will</b> be modified
         * @param <T>      can be any non-primitive type.
         * @return elements after shuffling it in-place
         */
        T[] ShuffleInPlace<T>(T[] elements);

        /**
         * Shuffle an array using the Fisher-Yates algorithm. DO NOT give the same array for both elements and
         * dest, since the prior contents of dest are rearranged before elements is used, and if they refer to the same
         * array, then you can end up with bizarre bugs where one previously-unique item shows up dozens of times. If
         * possible, create a new array with the same length as elements and pass it in as dest; the returned value can be
         * assigned to whatever you want and will have the same items as the newly-formed array.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         *
         * @param elements an array of T; will not be modified
         * @param <T>      can be any non-primitive type.
         * @param dest     Where to put the shuffle. If it does not have the same length as {@code elements}, this will use the
         *                 randomPortion method of this class to fill the smaller dest. MUST NOT be the same array as elements!
         * @return {@code dest} after modifications
         */
        T[] Shuffle<T>(T[] elements, T[] dest);
        /**
         * Shuffles a {@link Collection} of T using the Fisher-Yates algorithm and returns an ArrayList of T.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         * @param elements a Collection of T; will not be modified
         * @param <T>      can be any non-primitive type.
         * @return a shuffled ArrayList containing the whole of elements in pseudo-random order.
         */
        List<T> Shuffle<T>(IEnumerable<T> elements);

        /**
         * Shuffles a {@link Collection} of T using the Fisher-Yates algorithm and puts it in a buffer.
         * The result is allocated if {@code buf} is null or if {@code buf} isn't empty,
         * otherwise {@code elements} is poured into {@code buf}.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         * @param elements a Collection of T; will not be modified
         * @param buf a buffer as an ArrayList that will be filled with the shuffled contents of elements;
         *            if null or non-empty, a new ArrayList will be allocated and returned
         * @param <T>      can be any non-primitive type.
         * @return a shuffled ArrayList containing the whole of elements in pseudo-random order, which may be {@code buf}
         */
        List<T> Shuffle<T>(IEnumerable<T> elements, List<T> buf);
        /**
         * Shuffles a List of T items in-place using the Fisher-Yates algorithm.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         *
         * @param elements a List of T; <b>will</b> be modified
         * @param <T>      can be any non-primitive type.
         * @return elements after shuffling it in-place
         */
        List<T> ShuffleInPlace<T>(List<T> elements);
        /**
         * Shuffles an IOrdered data structure, such as an IndexedSet or IndexedDictionary, in-place using the Fisher-Yates algorithm.
         * <br>
         * <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Wikipedia has more on this algorithm</a>.
         *
         * @param ordered an IOrdered, such as an IndexedSet or IndexedDictionary; <b>will</b> be modified
         * @param <T>     disregarded; the type won't affect how this behaves
         * @return ordered after shuffling it in-place
         */
        void ShuffleInPlace<T>(IOrdered<T> ordered);
        /**
         * Gets a random portion of data (an array), assigns that portion to output (an array) so that it fills as much as
         * it can, and then returns output. Will only use a given position in the given data at most once.
         * 
         * @param data   an array of T; will not be modified.
         * @param output an array of T that will be overwritten; should always be instantiated with the portion length
         * @param <T>    can be any non-primitive type.
         * @return output, after {@code Math.min(output.length, data.length)} unique items have been put into it from data
         */
        T[] RandomPortion<T>(T[] data, T[] output);

        /**
         * Get the current internal state of the IStatefulRNG as a string, which only has to encode the state so that
         * an IStatefulRNG implementation with the same class can load the state back with setState().
         * @return the current internal state of this object.
         */
        string StateCode { get; set; }

    }

}
