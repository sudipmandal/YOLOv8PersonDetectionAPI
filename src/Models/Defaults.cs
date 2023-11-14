namespace YoloPersonDetectionAPI.Models
{
    public class Defaults
    {
        public string variableName;
        public string defaultValue;
        public VARTYPE type;

        static List<Defaults> defaults = new List<Defaults>();

        public static List<Defaults> GetDefaults()
        {
            if (defaults.Count == 0)
            {
                defaults.Add(new Defaults() { variableName = Constants.SENSITIVITY, defaultValue = "0.5", type = VARTYPE.Numeric });
            }

            return defaults;
        }

        public static string GetDefault(string variableName)
        {
            var d = GetDefaults().Where(x => x.variableName == variableName).FirstOrDefault();
            if (d == null)
                throw new Exception($"No Default defined for ${variableName}");

            //return (T) Convert.ChangeType(d.defaultValue, typeof(T));
            return d.defaultValue;
        }
    }
}
