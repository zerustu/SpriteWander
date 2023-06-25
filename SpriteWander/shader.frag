#version 330 core
in vec2 texCoord;

uniform sampler2D texture0;

out vec4 FragColor;

void main()
{
    vec4 texColor = texture(texture0, texCoord);
    if (texColor.a < 0.1)
    {
        discard;
    }
    FragColor = texColor;
}