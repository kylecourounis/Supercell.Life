namespace Supercell.Life.Titan.Files.CsvReader
{
    using System;
    using System.Reflection;

    using Supercell.Life.Titan.Files.CsvData;
    using Supercell.Life.Titan.Logic;

    public class Row
    {
        public readonly int RowStart;
        public readonly Table Table;

        /// <summary>
        /// Gets the name of this row.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Table.GetValueAt(0, this.RowStart);
            }
        }

        /// <summary>
        /// Gets the row offset.
        /// </summary>
        public int Offset
        {
            get
            {
                return this.RowStart;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        public Row(Table table)
        {
            this.Table = table;
            this.RowStart = this.Table.GetColumnRowCount();
        }

        /// <summary>
        /// Gets the size of the array.
        /// </summary>
        public int GetArraySize(string name)
        {
            int index = this.Table.GetColumnIndexByName(name);
            return index != -1 ? this.Table.GetArraySizeAt(this, index) : 0;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string GetValue(string name, int level)
        {
            return this.Table.GetValue(name, level + this.RowStart);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData(IData data)
        {
            foreach (PropertyInfo property in data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (property.CanRead && property.CanWrite)
                {
                    if (property.PropertyType.IsArray)
                    {
                        Type elementType = property.PropertyType.GetElementType();

                        if (elementType == typeof(byte))
                        {
                            property.SetValue(data, this.LoadBoolArray(property.Name));
                        }

                        else if (elementType == typeof(int))
                        {
                            property.SetValue(data, this.LoadIntArray(property.Name));
                        }

                        else if (elementType == typeof(string))
                        {
                            property.SetValue(data, this.LoadStringArray(property.Name));
                        }

                        else
                        {
                            throw new Exception(elementType + "[] is not a valid array.");
                        }
                    }
                    else if (property.PropertyType.IsGenericType)
                    {
                        if (property.PropertyType == typeof(LogicArrayList<>))
                        {
                            Type listType = typeof(LogicArrayList<>);
                            Type[] generic = property.PropertyType.GetGenericArguments();
                            Type concreteType = listType.MakeGenericType(generic);
                            object newList = Activator.CreateInstance(concreteType);
                            MethodInfo add = concreteType.GetMethod("Add");
                            string indexerName = ((DefaultMemberAttribute) newList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;
                            PropertyInfo indexProperty = newList.GetType().GetProperty(indexerName);

                            for (int i = this.Offset; i < this.Offset + this.GetArraySize(property.Name); i++)
                            {
                                string value = this.GetValue(property.Name, i - this.Offset);

                                if (value == string.Empty && i != this.Offset)
                                {
                                    value = indexProperty.GetValue(newList, new object[]
                                    {
                                        i - this.Offset - 1
                                    }).ToString();
                                }

                                if (string.IsNullOrEmpty(value))
                                {
                                    object @object = generic[0].IsValueType ? Activator.CreateInstance(generic[0]) : string.Empty;

                                    add.Invoke(newList, new[]
                                    {
                                        @object
                                    });
                                }
                                else
                                {
                                    add.Invoke(newList, new[]
                                    {
                                        Convert.ChangeType(value, generic[0])
                                    });
                                }
                            }

                            property.SetValue(data, newList);
                        }
                        else if (property.PropertyType == typeof(IData) || property.PropertyType.BaseType == typeof(IData))
                        {
                            this.LoadData((IData) property.GetValue(property));
                        }
                    }
                    else
                    {
                        string value = this.GetValue(property.Name, 0);

                        if (!string.IsNullOrEmpty(value))
                        {
                            property.SetValue(data, Convert.ChangeType(value, property.PropertyType));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the boolean array.
        /// </summary>
        private bool[] LoadBoolArray(string column)
        {
            bool[] array = new bool[this.GetArraySize(column)];

            for (int i = 0; i < array.Length; i++)
            {
                string value = this.GetValue(column, i);

                if (!string.IsNullOrEmpty(value))
                {
                    if (bool.TryParse(value, out bool boolean))
                    {
                        array[i] = boolean;
                    }
                    else 
                        throw new Exception("Value '" + value + "' is not Boolean Value.");
                }
            }

            return array;
        }

        /// <summary>
        /// Loads the integer array.
        /// </summary>
        private int[] LoadIntArray(string column)
        {
            int[] array = new int[this.GetArraySize(column)];

            for (int i = 0; i < array.Length; i++)
            {
                string value = this.GetValue(column, i);

                if (!string.IsNullOrEmpty(value))
                {
                    if (int.TryParse(value, out int number))
                    {
                        array[i] = number;
                    }
                    else
                        throw new Exception("Value '" + value + "' is not Integer Value.");
                }
            }

            return array;
        }

        /// <summary>
        /// Loads the string array.
        /// </summary>
        private string[] LoadStringArray(string column)
        {
            string[] array = new string[this.GetArraySize(column)];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = this.GetValue(column, i);
            }

            return array;
        }
    }
}