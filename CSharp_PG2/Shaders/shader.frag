#version 330

// Needs to have same name as in the vertex shader
in vec3 color;
in vec3 Normal;
// Imports the current position from the Vertex Shader
in vec3 crntPos;
in vec2 texCoord;

out vec4 outputColor;


uniform vec3 lightColor;
// Gets the position of the light from the main function
uniform vec3 lightPos;
// Gets the position of the camera from the main function
uniform vec3 camPos;
uniform sampler2D texture0;

void main()
{
    /*vec4 col = vec4(color,1.0f);*/
    /*vec3 normal = normalize(Normal);*/
    /*vec3 lightDirection = normalize(lightPos - crntPos);*/
    /*float diffuse = max(dot(normal, lightDirection), 0.0f);*/

    /*  outputColor =col * lightColor * diffuse;*/

    //FragColor=lightColor*diffuse;
    float ambientStrength = 0.5;
    vec3 ambient = ambientStrength * lightColor;
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - crntPos);

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
    float specularStrength = 0.5;
    vec3 viewDir = normalize(camPos - crntPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

    vec3 result =  color;
    outputColor = texture(texture0,texCoord);
}                        