#version 330
in vec2 texCoord;

uniform sampler2D texture0;

out vec4 FragColor;

void main()
{
    FragColor = texture(texture0, texCoord);
}