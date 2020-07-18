namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Collections.Generic;
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Avatar.Items;

    internal class LogicDataSlot : Dictionary<int, Item>
    {
        internal LogicClientAvatar Avatar;

        /// <summary>
        /// Gets the checksum.
        /// </summary>
        internal int Checksum
        {
            get
            {
                return this.Values.Sum(item => item.Id + item.Count);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataSlot"/> class. 
        /// </summary>
        internal LogicDataSlot(int capacity = 50) : base(capacity)
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataSlot"/> class.
        /// </summary>
        internal LogicDataSlot(LogicClientAvatar avatar, int capacity = 50) : this(capacity)
        {
            this.Avatar = avatar;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal virtual void Initialize()
        {
            // Initialize.
        }

        /// <summary>
        /// Adds an instance of <see cref="Item"/> using the specified GlobalID and count.
        /// </summary>
        internal void AddItem(int data, int count)
        {
            if (this.TryGetValue(data, out Item current))
            {
                current.Count += count;
            }
            else
                this.Add(data, new Item(data, count));
        }

        /// <summary>
        /// Adds the specified <see cref="Item"/>.
        /// </summary>
        internal void AddItem(Item item)
        {
            if (this.TryGetValue(item.Id, out Item current))
            {
                current.Count += item.Count;
            }
            else
                this.Add(item.Id, item);
        }

        /// <summary>
        /// Gets the <see cref="Item"/> with the specified GlobalID.
        /// </summary>
        internal Item Get(int data)
        {
            return this.TryGetValue(data, out Item item) ? item : null;
        }

        /// <summary>
        /// Gets the count of the <see cref="Item"/> with the specified GlobalID.
        /// </summary>
        internal int GetCount(int data)
        {
            return this.TryGetValue(data, out Item item) ? item.Count : 0;
        }

        /// <summary>
        /// Subtracts the specified count from the instance of <see cref="Item"/> obtained with the specified GlobalID.
        /// </summary>
        internal void Remove(int data, int count)
        {
            if (this.TryGetValue(data, out Item current))
            {
                current.Count -= count;
            }
        }

        /// <summary>
        /// Subtracts the specified count using the specified instance of <see cref="Item"/>.
        /// </summary>
        internal void Remove(Item item, int count)
        {
            if (this.TryGetValue(item.Id, out Item current))
            {
                current.Count -= count;
            }
        }

        /// <summary>
        /// Sets the count for the <see cref="Item"/> with the specified GlobalID.
        /// </summary>
        internal void Set(int data, int count)
        {
            if (this.TryGetValue(data, out Item current))
            {
                current.Count = count;
            }
            else
                this.Add(data, new Item(data, count));
        }

        /// <summary>
        /// Sets an instance of item <see cref="Item"/>.
        /// </summary>
        internal void Set(Item item)
        {
            if (this.TryGetValue(item.Id, out Item current))
            {
                current.Count = item.Count;
            }
            else
                this.Add(item.Id, new Item(item.Id, item.Count));
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream stream)
        {
            this.Clear();

            int count = stream.ReadInt();

            if (count > 0)
            {
                do
                {
                    Item item = new Item();

                    item.Decode(stream);

                    if (item.Id > 0)
                    {
                        this.Add(item.Id, item);
                    }
                } while (--count > 0);
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode(ByteStream stream)
        {
            stream.WriteInt(this.Count);

            foreach (Item item in this.Values)
            {
                item.Encode(stream);
            }
        }
    }
}