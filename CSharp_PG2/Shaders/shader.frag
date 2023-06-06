#version 330

in vec2 texCoord;
//in vec3 Normal;
//in vec3 crntPos;

out vec4 FragColor;


//uniform vec3 lightColor;
//uniform vec3 lightPos;
//uniform vec3 camPos;
uniform sampler2D texture0;

void main()
{
    /*vec4 col = vec4(color,1.0f);*/
    /*vec3 normal = normalize(Normal);*/
    /*vec3 lightDirection = normalize(lightPos - crntPos);*/
    /*float diffuse = max(dot(normal, lightDirection), 0.0f);*/

    /*  outputColor =col * lightColor * diffuse;*/

    //FragColor=lightColor*diffuse;
//    float ambientStrength = 0.5;
//    vec3 ambient = ambientStrength * lightColor;
//    vec3 norm = normalize(Normal);
//    vec3 lightDir = normalize(lightPos - crntPos);
//
//    float diff = max(dot(norm, lightDir), 0.0);
//    vec3 diffuse = diff * lightColor;
//    float specularStrength = 0.5;
//    vec3 viewDir = normalize(camPos - crntPos);
//    vec3 reflectDir = reflect(-lightDir, norm);
//    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
//    vec3 specular = specularStrength * spec * lightColor;
//
//    vec3 result = (ambient + diffuse + specular) * color;
    FragColor = texture(texture0, texCoord);
}                        