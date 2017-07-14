namespace Sendy.Client.Model
{
	/// <summary>
	/// Default data type is text
	/// </summary>
    public class CustomField
    {
	    public enum DataTypes
	    {
		    Text,
		    Date
	    }

		public string Name { get; set; }
		/// <summary>
		/// Default datatype is text
		/// </summary>
	    public DataTypes DataType { get; set; }

	    public CustomField()
	    {
		    DataType = DataTypes.Text;
	    }

	    public CustomField(string name) : this()
	    {
		    Name = name;
	    }

	    public CustomField(string name, DataTypes dataType)
	    {
		    Name = name;
		    DataType = dataType;
	    }
	}
}
