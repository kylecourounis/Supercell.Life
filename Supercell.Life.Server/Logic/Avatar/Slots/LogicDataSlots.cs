namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Collections.Generic;
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Avatar;

    internal class LogicDataSlots : Dictionary<int, LogicDataSlot>
    {
        internal LogicClientAvatar Avatar;

        /// <summary>
        /// Gets the checksum.
        /// </summary>
        internal int Checksum
        {
            get
            {
                return this.Values.Sum(slot => slot.Checksum);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataSlots"/> class. 
        /// </summary>
        internal LogicDataSlots(int capacity = 50) : base(capacity)
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataSlots"/> class.
        /// </summary>
        internal LogicDataSlots(LogicClientAvatar avatar, int capacity = 50) : this(capacity)
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
        /// Adds an instance of <see cref="LogicDataSlot"/> using the specified GlobalID and count.
        /// </summary>
        internal void AddItem(int data, int count)
        {
            if (this.TryGetValue(data, out LogicDataSlot current))
            {
                current.Count += count;
            }
            else
            {
                this.Add(data, new LogicDataSlot(data, count));
            }
        }

        /// <summary>
        /// Adds the specified <see cref="LogicDataSlot"/>.
        /// </summary>
        internal void AddItem(LogicDataSlot item)
        {
            if (this.TryGetValue(item.Id, out LogicDataSlot current))
            {
                current.Count += item.Count;
            }
            else
            {
                this.Add(item.Id, item);
            }
        }

        /// <summary>
        /// Gets the <see cref="LogicDataSlot"/> with the specified GlobalID.
        /// </summary>
        internal LogicDataSlot Get(int data)
        {
            return this.TryGetValue(data, out LogicDataSlot slot) ? slot : null;
        }

        /// <summary>
        /// Gets the count of the <see cref="LogicDataSlot"/> with the specified GlobalID.
        /// </summary>
        internal int GetCount(int data)
        {
            return this.TryGetValue(data, out LogicDataSlot slot) ? slot.Count : 0;
        }

        /// <summary>
        /// Subtracts the specified count from the instance of <see cref="LogicDataSlot"/> obtained with the specified GlobalID.
        /// </summary>
        internal void Remove(int data, int count)
        {
            if (this.TryGetValue(data, out LogicDataSlot current))
            {
                current.Count -= count;
            }
        }

        /// <summary>
        /// Subtracts the specified count using the specified instance of <see cref="LogicDataSlot"/>.
        /// </summary>
        internal void Remove(LogicDataSlot item, int count)
        {
            if (this.TryGetValue(item.Id, out LogicDataSlot current))
            {
                current.Count -= count;
            }
        }

        /// <summary>
        /// Sets the count for the <see cref="LogicDataSlot"/> with the specified GlobalID.
        /// </summary>
        internal void Set(int data, int count)
        {
            if (this.TryGetValue(data, out LogicDataSlot current))
            {
                current.Count = count;
            }
            else
            {
                this.Add(data, new LogicDataSlot(data, count));
            }
        }

        /// <summary>
        /// Sets an instance of item <see cref="LogicDataSlot"/>.
        /// </summary>
        internal void Set(LogicDataSlot slot)
        {
            if (this.TryGetValue(slot.Id, out LogicDataSlot current))
            {
                current.Count = slot.Count;
            }
            else
            {
                this.Add(slot.Id, new LogicDataSlot(slot.Id, slot.Count));
            }
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
                    LogicDataSlot slot = new LogicDataSlot();

                    slot.Decode(stream);

                    if (slot.Id > 0)
                    {
                        this.Add(slot.Id, slot);
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

            foreach (LogicDataSlot slot in this.Values)
            {
                slot.Encode(stream);
            }
        }
    }
}