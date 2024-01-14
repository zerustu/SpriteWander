package spritewander.utils;

public class utils {
    /**
     * @param <T> type of variables, must implement Comparable<T>
     * @param value value to clamp
     * @param min minimal value of the interval
     * @param max maximum value of the interval
     * @return return the closest value of value in the [min, max] segment, return value if it is in [min, max]
     * @throws Exception raise an exception if max < min
     */
    public static <T extends Comparable<T>> T Clamp(T value, T min, T max) throws Exception
    {
        if (max.compareTo(min) < 0) throw new Exception("min must be smaller than max for clamp function (min: " + min + ", max: " + max + ")");
        if (value.compareTo(min) < 0) return min;
        if (value.compareTo(max) > 0) return max;
        else return value;
    }
}
