namespace EFTask.Extentions
{
    public static class ReflectionExtions
    {
        public static string GetPropertyValue<T>(this T item,string PropertyName)
        {
            return item.GetType().GetProperty(PropertyName).GetValue(item,null).ToString();
        }
    }
}
