public class ArrayUtil
{
    public static int arrayContains(string[] array, string _object)
    {
        for(int i = 0; i < array.Length; ++i)
            if(array[i] == _object)
                return i;
        return -1;
    }
}
