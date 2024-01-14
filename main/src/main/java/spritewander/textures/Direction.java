package spritewander.textures;

public enum Direction
{
    Down(0),
    DownRight(1),
    Right(2),
    UpRight(3),
    Up(4),
    UpLeft(5),
    Left(6),
    DownLeft(7);

    private final int value;

    Direction(int value) { 
        this.value = value; 
    }

    public int getValue(){return this.value;}

    public int[] ToVect() {
        switch (this.value) {
            case 0:
            default:
                return new int[]{0, -1};
            case 1:
                return new int[]{1, -1};
            case 2:
                return new int[]{1, 0};
            case 3:
                return new int[]{1, 1};
            case 4:
                return new int[]{0, 1};
            case 5:
                return new int[]{-1, 1};
            case 6:
                return new int[]{-1, 0};
            case 7:
                return new int[]{-1, -1};
        }
    }

    public static Direction fromInt(int value) {
        int v = value;
        while (v < 0) {
            v += 8;
        }
        while (v > 7) {
            v -= 8;
        }
        for (Direction direction : values()) {
            if (direction.value == v) {
                return direction;
            }
        }
        throw new IllegalArgumentException("Invalid Direction value: " + value);
    }
}
