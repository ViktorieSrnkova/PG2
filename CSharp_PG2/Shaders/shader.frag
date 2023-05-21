#version 330

// Needs to have same name as in the vertex shader
in vec3 color;
in vec3 Normal;
// Imports the current position from the Vertex Shader
in vec3 crntPos;

out vec4 outputColor;

uniform vec4 lightColor;
// Gets the position of the light from the main function
uniform vec3 lightPos;
// Gets the position of the camera from the main function
uniform vec3 camPos;

void main()
{
    vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(lightPos - crntPos);
	float diffuse = max(dot(normal, lightDirection), 0.0f);
	
    outputColor = vec4(color * diffuse, 1.0);
}