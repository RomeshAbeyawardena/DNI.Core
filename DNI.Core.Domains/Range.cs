﻿namespace DNI.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Range<TStruct>
        where TStruct : struct, IComparable, IComparable<TStruct>, IConvertible, IEquatable<TStruct>, IFormattable
    {
        private readonly Func<TStruct, TStruct, IEnumerable<TStruct>> getSequence;

        protected Range(TStruct start, TStruct end, Func<TStruct, TStruct, IEnumerable<TStruct>> getSequence)
        {
            this.getSequence = getSequence;
            Start = start;
            End = end;
        }

        public TStruct Start { get; }

        public TStruct End { get; }

        public IEnumerable<TStruct> ToSequence()
        {
            return getSequence(Start, End);
        }
    }

    public class Range : Range<byte>
    {
        public Range(byte start, byte end)
            : base(start, end, (start, end) =>
            {
                IEnumerable<byte> rangeArray = Array.Empty<byte>();
                for (var index = start; index <= end; index++)
                {
                    rangeArray = rangeArray.Append(index);
                }

                return rangeArray;
            })
        {
            // do
        }
    }
}
