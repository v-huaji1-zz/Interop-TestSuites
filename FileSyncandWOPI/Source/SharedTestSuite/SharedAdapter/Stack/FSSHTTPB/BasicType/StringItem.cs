namespace Microsoft.Protocols.TestSuites.SharedAdapter
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The count and content of an arbitrary wide character string.
    /// </summary>
    public class StringItem : BasicObject
    {
        /// <summary>
        /// Initializes a new instance of the StringItem class.
        /// </summary>
        public StringItem(StringItem stringItem)
        {
            this.Count = new Compact64bitInt();
        }

        /// <summary>
        /// Initializes a new instance of the StringItem class, this is default constructor.
        /// </summary>
        public StringItem()
        {
        }

        /// <summary>
        /// Gets or sets the count of character.
        /// </summary>
        public Compact64bitInt Count { get; set; }

        /// <summary>
        /// Gets or sets an array of UTF-16 characters that specify the string. It MUST NOT be null-terminated.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// This method is used to convert the element of StringItem basic object into a byte List.
        /// </summary>
        /// <returns>Return the byte list which store the byte information of StringItem.</returns>
        public override List<byte> SerializeToByteList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to deserialize the StringItem basic object from the specified byte array and start index.
        /// </summary>
        /// <param name="byteArray">Specify the byte array.</param>
        /// <param name="startIndex">Specify the start index from the byte array.</param>
        /// <returns>Return the length in byte of the StringItem basic object.</returns>
        protected override int DoDeserializeFromByteArray(byte[] byteArray, int startIndex)
        {
            int index = startIndex;
            this.Count = BasicObject.Parse<Compact64bitInt>(byteArray, ref index);
            this.Content = System.Text.Encoding.Unicode.GetString(byteArray, index, Convert.ToInt32(this.Count.DecodedValue * 2));
            index += (int)this.Count.DecodedValue * 2;
            return index - startIndex;
        }
    }

    /// <summary>
    /// This class is used to specify the array of String Items.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Easy to maintain one group of classes in one .cs file.")]
    public class StringItemArray : BasicObject
    {
        /// <summary>
        /// Initializes a new instance of the StringItemArray class.
        /// </summary>
        /// <param name="count">Specify the number of StringItem in the StringItem array.</param>
        /// <param name="content">Specify the list of StringItem.</param>
        public StringItemArray(ulong count, List<StringItem> content)
        {
            this.Count = count;
            this.Content = content;
        }

        /// <summary>
        /// Initializes a new instance of the StringItemArray class, this is copy constructor.
        /// </summary>
        /// <param name="stringItemArray">Specify the StringItemArray.</param>
        public StringItemArray(StringItemArray stringItemArray)
        {
            this.Count = stringItemArray.Count;
            if (stringItemArray.Content != null)
            {
                foreach (StringItem stringItem in stringItemArray.Content)
                {
                    this.Content.Add(new StringItem(stringItem));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the StringItemArray class, this is default constructor.
        /// </summary>
        public StringItemArray()
        {
            this.Content = new List<StringItem>();
        }

        /// <summary>
        /// Gets or sets a compact unsigned 64-bit integer that specifies the count of String Items in the array. 
        /// </summary>
        public ulong Count { get; set; }

        /// <summary>
        /// Gets or sets a String Item list that specifies a list of string items.
        /// </summary>
        public List<StringItem> Content { get; set; }

        /// <summary>
        /// This method is used to convert the element of StringItemArray basic object into a byte List.
        /// </summary>
        /// <returns>Return the byte list which store the byte information of StringItemArray.</returns>
        public override List<byte> SerializeToByteList()
        {
            List<byte> byteList = new List<byte>();
            byteList.AddRange((new Compact64bitInt(this.Count)).SerializeToByteList());
            if (this.Content != null)
            {
                foreach (StringItem extendGuid in this.Content)
                {
                    byteList.AddRange(extendGuid.SerializeToByteList());
                }
            }

            return byteList;
        }

        /// <summary>
        /// This method is used to deserialize the StringItemArray basic object from the specified byte array and start index.
        /// </summary>
        /// <param name="byteArray">Specify the byte array.</param>
        /// <param name="startIndex">Specify the start index from the byte array.</param>
        /// <returns>Return the length in byte of the StringItemArray basic object.</returns>
        protected override int DoDeserializeFromByteArray(byte[] byteArray, int startIndex)
        {
            int index = startIndex;

            this.Count = BasicObject.Parse<Compact64bitInt>(byteArray, ref index).DecodedValue;

            for (ulong i = 0; i < this.Count; i++)
            {
                this.Content.Add(BasicObject.Parse<StringItem>(byteArray, ref index));
            }

            return index - startIndex;
        }
    }
}