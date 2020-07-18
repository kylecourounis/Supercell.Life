namespace Supercell.Life.Server.Files.CsvHelpers
{
    using System;
    using System.Reflection;

    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;

    internal class LogicData
    {
        internal LogicDataTable DataTable;
        internal Row Row;

        internal readonly int GlobalID;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicData"/> class.
        /// </summary>
        internal LogicData()
        {
            // LogicData.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicData"/> class.
        /// </summary>
        internal LogicData(Row row, LogicDataTable dataTable)
        {
            this.Row       = row;
            this.DataTable = dataTable;
            this.GlobalID  = dataTable.Datas.Size + 1000000 * dataTable.Index;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        internal static void Load(LogicData data, Type type, Row row)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.PropertyType.IsGenericType)
                {
                    Type listType              = typeof(LogicArrayList<>);
                    Type[] generic             = property.PropertyType.GetGenericArguments();
                    Type concreteType          = listType.MakeGenericType(generic);
                    object newList             = Activator.CreateInstance(concreteType);
                    MethodInfo add             = concreteType.GetMethod("Add");
                    string indexerName         = ((DefaultMemberAttribute) newList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;
                    PropertyInfo indexProperty = newList.GetType().GetProperty(indexerName);

                    for (int i = row.Offset; i < row.Offset + row.GetArraySize(property.Name); i++)
                    {
                        string value = row.GetValue(property.Name, i - row.Offset);

                        if (value == string.Empty && i != row.Offset)
                        {
                            value = indexProperty?.GetValue(newList, new object[]
                            {
                                i - row.Offset - 1
                            }).ToString();
                        }

                        if (string.IsNullOrEmpty(value))
                        {
                            object obj = generic[0].IsValueType ? Activator.CreateInstance(generic[0]) : string.Empty;

                            add?.Invoke(newList, new[]
                            {
                                obj
                            });
                        }
                        else
                        {
                            add?.Invoke(newList, new[]
                            {
                                Convert.ChangeType(value, generic[0])
                            });
                        }
                    }

                    property.SetValue(data, newList);
                }
                else
                {
                    property.SetValue(data, string.IsNullOrEmpty(row.GetValue(property.Name, 0)) ? null : Convert.ChangeType(row.GetValue(property.Name, 0), property.PropertyType), null);
                }
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        internal int GetID()
        {
            return CsvHelpers.GlobalID.GetID(this.GlobalID);
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        internal int GetDataType()
        {
            return CsvHelpers.GlobalID.GetType(this.GlobalID);
        }
    }
}