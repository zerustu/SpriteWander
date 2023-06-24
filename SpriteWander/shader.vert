#version 330 core
layout(location = 1) in vec2 aPosition;
layout(location = 2) in vec2 aTexCoord;

out vec2 texCoord;

void main()
{
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 0.0 , 1.0);
}