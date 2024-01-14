package spritewander.textures;

public enum AnimEvent
    {
        /// <summary>
        /// keep playing the current animation
        /// </summary>
        Nothing,
        /// <summary>
        /// keep playing the current animation but reset the animation timer to loop again (change the animation if played to many time)
        /// </summary>
        Reset,
        /// <summary>
        /// stop playing that animation and change to the next one
        /// </summary>
        End
    }