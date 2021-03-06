﻿using System;
using System.Collections.Generic;
using System.Text;
using SquidLib.SquidMath;

namespace SquidLib.SquidGrid {
    public static class LineKit {
        public static readonly char[] LightAlt = " ╴╵┘╶─└┴╷┐│┤┌┬├┼".ToCharArray(),
                           HeavyAlt = " ╸╹┛╺━┗┻╻┓┃┫┏┳┣╋".ToCharArray(),
                           Light = " ─│┘──└┴│┐│┤┌┬├┼".ToCharArray(),
                           Heavy = " ━┃┛━━┗┻┃┓┃┫┏┳┣╋".ToCharArray();
        //                          0123456789ABCDEF
        public static readonly IndexedDictionary<char, char> ToHeavy = new IndexedDictionary<char, char>(16) {
            [' '] = ' ',
            ['╴'] = '╸',
            ['╵'] = '╹',
            ['┘'] = '┛',
            ['╶'] = '╺',
            ['─'] = '━',
            ['└'] = '┗',
            ['┴'] = '┻',
            ['╷'] = '╻',
            ['┐'] = '┓',
            ['│'] = '┃',
            ['┤'] = '┫',
            ['┌'] = '┏',
            ['┬'] = '┳',
            ['├'] = '┣',
            ['┼'] = '╋',
        };
        public static readonly IndexedDictionary<char, char> ToLight = new IndexedDictionary<char, char>(16) {
            [' '] = ' ',
            ['╸'] = '╴',
            ['╹'] = '╵',
            ['┛'] = '┘',
            ['╺'] = '╶',
            ['━'] = '─',
            ['┗'] = '└',
            ['┻'] = '┴',
            ['╻'] = '╷',
            ['┓'] = '┐',
            ['┃'] = '│',
            ['┫'] = '┤',
            ['┏'] = '┌',
            ['┳'] = '┬',
            ['┣'] = '├',
            ['╋'] = '┼',
        };
        /**
         * A constant that represents the encoded pattern for a 4x4 square with all lines possible except those that
         * would extend to touch cells adjacent to the 4x4 area. Meant to restrict cells within the square area by using
         * bitwise AND with an existing encoded pattern as another ulong, as with {@code LineKit.InteriorSquare & encoded}.
         * If you limit the area to the square with this, you may sometimes want to add a border, and for that you can use
         * {@link #ExteriorSquare} and bitwise OR that with the restricted area.
         * <br>This looks like: 
         * <pre>
         * "┌┬┬┐"
         * "├┼┼┤"
         * "├┼┼┤"
         * "└┴┴┘"
         * </pre>
         */
        public const ulong InteriorSquare = 0x3776BFFEBFFE9DDCUL,
        /**
         * A constant that represents the encoded pattern for a 4x4 square with only the lines along the border. Meant to
         * either restrict cells to the border by using bitwise AND with an existing encoded pattern as another ulong, as
         * with {@code LineKit.ExteriorSquare & encoded}, or to add a border to an existing pattern with bitwise OR, as with
         * {@code LineKit.ExteriorSquare | encoded}.
         * <br>This looks like: 
         * <pre>
         * "┌──┐"
         * "│  │"
         * "│  │"
         * "└──┘"
         * </pre>
         */
        ExteriorSquare = 0x3556A00AA00A955CUL,
        /**
         * A constant that represents the encoded pattern for a 4x4 plus pattern with only the lines along the border. This
         * pattern has no lines in the corners of the 4x4 area, but has some lines in all other cells, though none that
         * would touch cells adjacent to this 4x4 area. Meant to restrict cells to the border by using bitwise AND with an
         * existing encoded pattern as another ulong, as with {@code LineKit.InteriorPlus & encoded}.
         * <br>This looks like: 
         * <pre>
         * " ┌┐ "
         * "┌┼┼┐"
         * "└┼┼┘"
         * " └┘ "
         * </pre>
         */
        InteriorPlus = 0x03603FF69FFC09C0UL,
        /**
         * A constant that represents the encoded pattern for a 4x4 plus pattern with only the lines along the border. This
         * pattern has no lines in the corners of the 4x4 area, but has some lines in all other cells, though none that
         * would touch cells adjacent to this 4x4 area. Meant to either restrict cells to the border by using bitwise AND
         * with an existing encoded pattern as another ulong, as with {@code LineKit.ExteriorPlus & encoded}, or to add a
         * border to an existing pattern with bitwise OR, as with {@code LineKit.ExteriorPlus | encoded}.
         * <br>This looks like: 
         * <pre>
         * " ┌┐ "
         * "┌┘└┐"
         * "└┐┌┘"
         * " └┘ "
         * </pre>
         */
        ExteriorPlus = 0x03603C96963C09C0UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 square. No lines will touch
         * the upper or left borders, but they do extend into the lower and right borders. This is expected to be flipped
         * using {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make the other corners. If middle
         * pieces are wanted that touch everything but the upper border, you can use
         * {@code (LineKit.InteriorSquareLarge | LineKit.flipHorizontal4x4(LineKit.InteriorSquareLarge))}. If you want it to
         * touch everything but the left border, you can use
         * {@code (LineKit.InteriorSquareLarge | LineKit.flipVertical4x4(LineKit.InteriorSquareLarge))}.
         * <br>This looks like: 
         * <pre>
         * "┌┬┬┬"
         * "├┼┼┼"
         * "├┼┼┼"
         * "├┼┼┼"
         * </pre>
         * @see #InteriorSquare The docs here cover how to use this as a mask with bitwise AND.
         */
        InteriorSquareLarge = 0xFFFEFFFEFFFEDDDCUL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 square border. No lines will
         * touch the upper or left borders, but they do extend into the lower and right borders. The entirety of this
         * pattern is one right-angle. This is expected to be flipped using {@link #flipHorizontal4x4(ulong)} and/or
         * {@link #flipVertical4x4(ulong)} to make the other corners.
         * <br>This looks like: 
         * <pre>
         * "┌───"
         * "│   "
         * "│   "
         * "│   "
         * </pre>
         * @see #ExteriorSquare The docs here cover how to use this as a mask with bitwise AND or to insert it with OR.
         */
        ExteriorSquareLarge = 0x000A000A000A555CUL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of a 6x6 square centered in an 8x8
         * space. A 3x3 square will be filled of the 4x4 area this represents. No lines will touch the upper or left
         * borders, but they do extend into the lower and right borders. This is expected to be flipped using
         * {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other corners.
         * <br>This looks like: 
         * <pre>
         * "    "
         * " ┌┬┬"
         * " ├┼┼"
         * " ├┼┼"
         * </pre>
         * @see #InteriorSquare The docs here cover how to use this as a mask with bitwise AND.
         * 
         */
        ShallowInteriorSquareLarge = 0xFFE0FFE0DDC00000UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of a 6x6 square border centered in an
         * 8x8 space. This consists of a 3-cell-ulong vertical line and a 3-cell-ulong horizontal line. No lines will touch
         * the upper or left borders, but they do extend into the lower and right borders. The entirety of this
         * pattern is one right-angle. This is expected to be flipped using {@link #flipHorizontal4x4(ulong)} and/or
         * {@link #flipVertical4x4(ulong)} to make the other corners.
         * <br>This looks like: 
         * <pre>
         * "    "
         * " ┌──"
         * " │  "
         * " │  "
         * </pre>
         * @see #ExteriorSquare The docs here cover how to use this as a mask with bitwise AND or to insert it with OR.
         */
        ShallowExteriorSquareLarge = 0x00A000A055C00000UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of a 4x4 square centered in an 8x8
         * space. A 2x2 square will be filled of the 4x4 area this represents. No lines will touch the upper or left
         * borders, but they do extend into the lower and right borders. This is expected to be flipped using
         * {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other corners.
         * <br>This looks like: 
         * <pre>
         * "    "
         * "    "
         * "  ┌┬"
         * "  ├┼"
         * </pre>
         * @see #InteriorSquare The docs here cover how to use this as a mask with bitwise AND.
         */
        ShallowerInteriorSquareLarge = 0xFE00DC0000000000UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of a 4x4 square border centered in an
         * 8x8 space. This consists of a 2-cell-ulong vertical line and a 2-cell-ulong horizontal line. No lines will touch
         * the upper or left borders, but they do extend into the lower and right borders. The entirety of this
         * pattern is one right-angle. This is expected to be flipped using {@link #flipHorizontal4x4(ulong)} and/or
         * {@link #flipVertical4x4(ulong)} to make the other corners.
         * <br>This looks like: 
         * <pre>
         * "    "
         * "    "
         * "  ┌─"
         * "  │ "
         * </pre>
         * @see #ExteriorSquare The docs here cover how to use this as a mask with bitwise AND or to insert it with OR.
         */
        ShallowerExteriorSquareLarge = 0x0A005C0000000000UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 plus shape. No lines will
         * touch the upper or left borders, but they do extend into the lower and right borders. This pattern leaves the
         * upper left 2x2 area blank, and touches all of the lower and right borders. This is expected to be flipped using
         * {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other corners.
         * @see #InteriorPlus The docs here cover how to use this as a mask with bitwise AND.
         * <br>This looks like: 
         * <pre>
         * "  ┌┬"
         * "  ├┼"
         * "┌┬┼┼"
         * "├┼┼┼"
         * </pre>
         */
        InteriorPlusLarge = 0xFFFEFFDCFE00DC00UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 plus shape border. No lines
         * will touch the upper or left borders, but they do extend into the lower and right borders. This pattern leaves
         * the upper left 2x2 area blank, as well as all but one each of the bottom and right border cells. This is expected
         * to be flipped using {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other corners.
         * <br>This looks like: 
         * <pre>
         * "  ┌─"
         * "  │ "
         * "┌─┘ "
         * "│   "
         * </pre>
         * @see #ExteriorPlus The docs here cover how to use this as a mask with bitwise AND or to insert it with OR.
         */
        ExteriorPlusLarge = 0x000A035C0A005C00UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 circle shape. No lines will
         * touch the upper or left borders, but they do extend into the lower and right borders. This is expected to be
         * flipped using {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other corners.
         * <br>This looks like: 
         * <pre>
         * "  ┌┬"
         * " ┌┼┼"
         * "┌┼┼┼"
         * "├┼┼┼"
         * </pre>
         * @see #InteriorPlus The docs here cover how to use this as a mask with bitwise AND.
         */
        InteriorCircleLarge = 0xFFFEFFFCFFC0DC00UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 circular border. No lines
         * will touch the upper or left borders, but they do extend into the lower and right borders. The entirety of this
         * pattern is one curving line. This is expected to be flipped using {@link #flipHorizontal4x4(ulong)} and/or
         * {@link #flipVertical4x4(ulong)} to make other corners.
         * <br>This looks like: 
         * <pre>
         * "  ┌─"
         * " ┌┘ "
         * "┌┘  "
         * "│   "
         * </pre>
         * @see #ExteriorPlus The docs here cover how to use this as a mask with bitwise AND or to insert it with OR.
         */
        ExteriorCircleLarge = 0x000A003C03C05C00UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 diamond shape. No lines will
         * touch the upper or left borders, but they do extend into the lower and right borders. This pattern leaves the
         * upper left 2x2 area blank, and touches all of the lower and right borders. This is expected to be flipped using
         * {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other corners. This has more of a
         * fine angle than {@link #InteriorPlusLarge}, which is otherwise similar.
         * <br>This looks like: 
         * <pre>
         * "   ┌"
         * "  ┌┼"
         * " ┌┼┼"
         * "┌┼┼┼"
         * </pre>
         * @see #InteriorPlus The docs here cover how to use this as a mask with bitwise AND.
         */
        InteriorDiamondLarge = 0xFFFCFFC0FC00C000UL,
        /**
         * A constant that represents the encoded pattern for the upper left 4x4 area of an 8x8 diamond shape border. No
         * lines will touch the upper or left borders, but they do extend into the lower and right borders. This pattern
         * leaves the upper left 2x2 area blank, as well as all but one each of the bottom and right border cells. This is
         * expected to be flipped using {@link #flipHorizontal4x4(ulong)} and/or {@link #flipVertical4x4(ulong)} to make other
         * corners. This has more of a fine angle than {@link #ExteriorPlusLarge}, which is otherwise similar.
         * <br>This looks like: 
         * <pre>
         * "   ┌"
         * "  ┌┘"
         * " ┌┘ "
         * "┌┘  "
         * </pre>
         * @see #ExteriorPlus The docs here cover how to use this as a mask with bitwise AND or to insert it with OR.
         */
        ExteriorDiamondLarge = 0x003C03C03C00C000UL;

        /**
         * Produces a 4x4 2D char array by interpreting the bits of the given ulong as line information. Uses the box drawing
         * chars from {@link #light}, which are compatible with most fonts.
         * @param encoded a ulong, which can be random, that encodes some pattern of (typically box drawing) characters
         * @return a 4x4 2D char array containing elements from Light, assigned based on encoded
         */
        public static Grid<char> Decode4x4(ulong encoded) => Decode4x4(encoded, Light);
        /**
         * Produces a 4x4 2D char array by interpreting the bits of the given ulong as line information. Uses the given char
         * array, which must have at least 16 elements and is usually one of {@link #light}, {@link #heavy},
         * {@link #lightAlt}, or {@link #heavyAlt}, with the last two usable only if using a font that supports the chars
         * {@code ╴╵╶╷} (this is true for Iosevka and Source Code Pro, for instance, but not Inconsolata or GoMono).
         * @param encoded a ulong, which can be random, that encodes some pattern of (typically box drawing) characters
         * @param symbols a 16-element-or-larger char array; usually a constant in this class like {@link #light}
         * @return a 4x4 2D char array containing elements from symbols assigned based on encoded
         */
        public static Grid<char> Decode4x4(ulong encoded, char[] symbols) {
            symbols = symbols ?? Light;
            Grid<char> v = new Grid<char>(4, 4, ' ');
            for (int i = 0; i < 16; i++) {
                v[i & 3, i >> 2] = symbols[encoded >> (i << 2) & 15UL];
            }
            return v;
        }
        /**
         * Fills a 4x4 area of the given 2D char array {@code into} by interpreting the bits of the given ulong as line
         * information. Uses the given char array {@code symbols}, which must have at least 16 elements and is usually one
         * of {@link #light}, {@link #heavy}, {@link #lightAlt}, or {@link #heavyAlt}, with the last two usable only if
         * using a font that supports the chars {@code ╴╵╶╷} (this is true for Iosevka and Source Code Pro, for instance,
         * but not Inconsolata or GoMono).
         * @param encoded a ulong, which can be random, that encodes some pattern of (typically box drawing) characters
         * @param symbols a 16-element-or-larger char array; usually a constant in this class like {@link #light}
         * @param into a 2D char array that will be modified in a 4x4 area
         * @param startX the first x position to modify in into
         * @param startY the first y position to modify in into
         * @return into, after modification
         */
        public static Grid<char> DecodeInto4x4(ulong encoded, char[] symbols, Grid<char> into, int startX, int startY) {
            symbols = symbols ?? Light;
            into = into ?? new Grid<char>(startX + 4, startY + 4, ' ', ' ');
            for (int i = 0; i < 16; i++) {
                into[(i & 3) + startX, (i >> 2) + startY] = symbols[encoded >> (i << 2) & 15UL];
            }
            return into;
        }

        /**
         * Reads a 2D char array {@code decoded}, which must be at least 4x4 in size, and returns a ulong that encodes the cells from 0,0 to
         * 3,3 in a way that this class can interpret and manipulate. The 2D array {@code decoded} must contain box drawing
         * symbols or hash marks, which can be any of those from {@link #light}, {@link #heavy}, {@link #lightAlt}, or {@link #heavyAlt}, plus '#'.
         * Valid chars are {@code ╴╵┘╶─└┴╷┐│┤┌┬├┼╸╹┛╺━┗┻╻┓┃┫┏┳┣╋#}; any other chars will be treated as empty space.
         * '#' will be treated the same as '┼' or '╋'.
         * @param decoded a 2D char array that must be at least 4x4 and should usually contain box drawing characters
         * @return a ulong that encodes the box drawing information in decoded so this class can manipulate it
         */
        public static ulong Encode4x4(Grid<char> decoded) => Encode4x4(decoded, 0, 0);
        public static ulong Encode4x4(Grid<char> decoded, int startX, int startY) {
                if (decoded is null)
                return 0UL;
            ulong v = 0UL;
            for (int i = 0; i < 16; i++) {
                switch (decoded[startX + (i & 3), startY + (i >> 2)]) {
                    // ╴╵┘╶─└┴╷┐│┤┌┬├┼
                    // ╸╹┛╺━┗┻╻┓┃┫┏┳┣╋
                    //0123456789ABCDEF
                    case '─':
                    case '━':
                        v |= 5UL << (i << 2);
                        break;
                    case '│':
                    case '┃':
                        v |= 10UL << (i << 2);
                        break;
                    case '┘':
                    case '┛':
                        v |= 3UL << (i << 2);
                        break;
                    case '└':
                    case '┗':
                        v |= 6UL << (i << 2);
                        break;
                    case '┐':
                    case '┓':
                        v |= 9UL << (i << 2);
                        break;
                    case '┌':
                    case '┏':
                        v |= 12UL << (i << 2);
                        break;
                    case '┴':
                    case '┻':
                        v |= 7UL << (i << 2);
                        break;
                    case '┤':
                    case '┫':
                        v |= 11UL << (i << 2);
                        break;
                    case '┬':
                    case '┳':
                        v |= 13UL << (i << 2);
                        break;
                    case '├':
                    case '┣':
                        v |= 14UL << (i << 2);
                        break;
                    case '┼':
                    case '╋':
                    case '#':
                        v |= 15UL << (i << 2);
                        break;
                    case '╴':
                    case '╸':
                        v |= 1UL << (i << 2);
                        break;
                    case '╵':
                    case '╹':
                        v |= 2UL << (i << 2);
                        break;
                    case '╶':
                    case '╺':
                        v |= 4UL << (i << 2);
                        break;
                    case '╷':
                    case '╻':
                        v |= 8UL << (i << 2);
                        break;
                }
            }
            return v;
        }

        /**
         * Makes a variant on the given encoded 4x4 pattern so the left side is flipped to the right side and vice versa.
         * @param encoded an encoded pattern ulong that represents a 4x4 area
         * @return a different encoded pattern ulong that represents the argument flipped left-to-right
         */
        public static ulong FlipHorizontal4x4(ulong encoded) {
            ulong v = 0UL;
            for (int i = 0, i4 = 0; i < 16; i++, i4 += 4) {
                v |= ((encoded >> i4 & 10UL) | ((encoded >> i4 & 1UL) << 2) | ((encoded >> i4 & 4UL) >> 2)) << (i + 3 - ((i & 3) << 1) << 2);
            }
            return v;
        }
        /**
         * Makes a variant on the given encoded 4x4 pattern so the top side is flipped to the bottom side and vice versa.
         * @param encoded an encoded pattern ulong that represents a 4x4 area
         * @return a different encoded pattern ulong that represents the argument flipped top-to-bottom
         */
        public static ulong FlipVertical4x4(ulong encoded) {
            ulong v = 0UL;
            for (int i = 0, i4 = 0; i < 16; i++, i4 += 4) {
                v |= ((encoded >> i4 & 5UL) | ((encoded >> i4 & 2UL) << 2) | ((encoded >> i4 & 8UL) >> 2)) << (i + 12 - ((i >> 2) << 3) << 2);
            }
            return v;
        }

        /**
         * Makes a variant on the given encoded 4x4 pattern so the x and y axes are interchanged, making the top side become
         * the left side and vice versa, while the bottom side becomes the right side and vice versa.
         * @param encoded an encoded pattern ulong that represents a 4x4 area
         * @return a different encoded pattern ulong that represents the argument transposed top-to-left and bottom-to-right
         */
        public static ulong Transpose4x4(ulong encoded) {
            ulong v = 0UL;
            for (int i4 = 0; i4 < 64; i4 += 4) {
                v |= (((encoded >> i4 & 5UL) << 1) | ((encoded >> i4 & 10UL) >> 1)) << ((i4 >> 2 & 12) | ((i4 & 12) << 2));
            }
            return v;
        }
        /**
         * Makes a variant on the given encoded 4x4 pattern so the lines are rotated 90 degrees clockwise, changing their
         * positions as well as what chars they will decode to. This can be called twice to get a 180 degree rotation, but
         * {@link #rotateCounterclockwise(ulong)} should be used for a 270 degree rotation.
         * @param encoded an encoded pattern ulong that represents a 4x4 area
         * @return a different encoded pattern ulong that represents the argument rotated 90 degrees clockwise
         */
        public static ulong RotateClockwise(ulong encoded) {
            // this is functionally equivalent to, but faster than, the following:
            // return flipHorizontal4x4(transpose4x4(encoded));
            ulong v = 0L;
            for (int i4 = 0; i4 < 64; i4 += 4) {
                v |= (((encoded >> i4 & 7UL) << 1) | ((encoded >> i4 & 8UL) >> 3)) << ((~i4 >> 2 & 12) | ((i4 & 12) << 2));
            }
            return v;
        }
        /**
         * Makes a variant on the given encoded 4x4 pattern so the lines are rotated 90 degrees counterclockwise, changing
         * their positions as well as what chars they will decode to. This can be called twice to get a 180 degree rotation,
         * but {@link #rotateClockwise(ulong)} should be used for a 270 degree rotation.
         * @param encoded an encoded pattern ulong that represents a 4x4 area
         * @return a different encoded pattern ulong that represents the argument rotated 90 degrees counterclockwise
         */
        public static ulong RotateCounterclockwise(ulong encoded) {
            // this is functionally equivalent to, but faster than, the following:
            // return flipVertical4x4(transpose4x4(encoded));
            ulong v = 0L;
            for (int i4 = 0; i4 < 64; i4 += 4) {
                v |= ((encoded >> (i4 + 1) & 7UL) | ((encoded >> i4 & 1UL) << 3)) << ((i4 >> 2 & 12) | ((~i4 & 12) << 2));
            }
            return v;
        }
        private static readonly char[] wallLookup = new char[]
        {
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '│', '─', '┘', '─', '┴', '┐', '┤', '┬', '┤',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '─', '┴',
                    '#', '│', '─', '└', '│', '│', '┌', '│', '─', '┘', '─', '┴', '┐', '┤', '─', '┘',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '─', '┐', '┤', '┬', '┬',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '┤', '┬', '┼',
                    '#', '│', '─', '└', '│', '│', '┌', '│', '─', '┘', '─', '─', '┐', '┤', '┬', '┐',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '│', '┬', '├',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '─', '┐', '│', '┬', '┌',
                    '#', '│', '─', '└', '│', '│', '┌', '├', '─', '┘', '─', '┴', '┐', '│', '─', '└',
                    '#', '│', '─', '└', '│', '│', '┌', '│', '─', '┘', '─', '─', '┐', '│', '─', ' '
        };

        /**
         * Takes a char[][] dungeon map that uses '#' to represent walls, and returns a new char[][] that uses unicode box
         * drawing characters to draw straight, continuous lines for walls, filling regions between walls (that were
         * filled with more walls before) with space characters, ' '. If keepSingleHashes is true, then '#' will be used if
         * a wall has no orthogonal wall neighbors; if it is false, then a horizontal line will be used for stand-alone
         * wall cells. If the lines "point the wrong way," such as having multiple horizontally adjacent vertical lines
         * where there should be horizontal lines, call transposeLines() on the returned map, which will keep the dimensions
         * of the map the same and only change the line chars. You will also need to call transposeLines if you call
         * hashesToLines on a map that already has "correct" line-drawing characters, which means hashesToLines should only
         * be called on maps that use '#' for walls. If you have a jumbled map that contains two or more of the following:
         * "correct" line-drawing characters, "incorrect" line-drawing characters, and '#' characters for walls, you can
         * reset by calling linesToHashes() and then potentially calling hashesToLines() again.
         *
         * @param map              a 2D char array indexed with x,y that uses '#' for walls
         * @param keepSingleHashes true if walls that are not orthogonally adjacent to other walls should stay as '#'
         * @return a copy of the map passed as an argument with box-drawing characters replacing '#' walls
         */
        public static Grid<char> HashesToLines(Grid<char> map, bool keepSingleHashes = true) {
            if (map is null) return null;
            Grid<char> dungeon = new Grid<char>(map);
            map.Outside = '#';
            int width = dungeon.Width, height = dungeon.Height;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (map[x, y] == '#') {
                        int q = 0;
                        q |= (map[x + 0, y - 1] == '#' || map[x + 0, y - 1] == '+' || map[x + 0, y - 1] == '/') ? 1 : 0;
                        q |= (map[x + 1, y + 0] == '#' || map[x + 1, y + 0] == '+' || map[x + 1, y + 0] == '/') ? 2 : 0;
                        q |= (map[x + 0, y + 1] == '#' || map[x + 0, y + 1] == '+' || map[x + 0, y + 1] == '/') ? 4 : 0;
                        q |= (map[x - 1, y + 0] == '#' || map[x - 1, y + 0] == '+' || map[x - 1, y + 0] == '/') ? 8 : 0;

                        q |= (map[x + 1, y - 1] == '#' || map[x + 1, y - 1] == '+' || map[x + 1, y - 1] == '/') ? 16 : 0;
                        q |= (map[x + 1, y + 1] == '#' || map[x + 1, y + 1] == '+' || map[x + 1, y + 1] == '/') ? 32 : 0;
                        q |= (map[x - 1, y + 1] == '#' || map[x - 1, y + 1] == '+' || map[x - 1, y + 1] == '/') ? 64 : 0;
                        q |= (map[x - 1, y - 1] == '#' || map[x - 1, y - 1] == '+' || map[x - 1, y - 1] == '/') ? 128 : 0;
                        if (!keepSingleHashes && wallLookup[q] == '#') {
                            dungeon[x, y] = '─';
                        } else {
                            dungeon[x, y] = wallLookup[q];
                        }
                    }
                }
            }
            map.Outside = dungeon.Outside;
            return dungeon;
        }

        /**
         * Reverses most of the effects of hashesToLines(). The only things that will not be reversed are the placement of
         * space characters in unreachable wall-cells-behind-wall-cells, which remain as spaces. This is useful if you
         * have a modified map that contains wall characters of conflicting varieties, as described in hashesToLines().
         *
         * @param map a 2D char array indexed with x,y that uses box-drawing characters for walls
         * @return a copy of the map passed as an argument with '#' replacing box-drawing characters for walls
         */
        public static Grid<char> LinesToHashes(Grid<char> map) {
            if (map is null) return null;
            Grid<char> dungeon = new Grid<char>(map);
            int width = dungeon.Width;
            int height = dungeon.Height;
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    switch (dungeon[i, j]) {
                        case ' ':
                        case '├':
                        case '┤':
                        case '┴':
                        case '┬':
                        case '┌':
                        case '┐':
                        case '└':
                        case '┘':
                        case '│':
                        case '─':
                        case '┼':
                            dungeon[i, j] = '#';
                            break;
                    }
                }
            }
            return dungeon;
        }

    }
}
