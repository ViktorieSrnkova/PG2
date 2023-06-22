#version 330 core
out vec4 FragColor;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

in vec3 FragPos;
in vec3 Normal;
in vec2 texCoord;

uniform Material material;
uniform vec3 camPos;
uniform sampler2D texture0;

struct DirLight{
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform DirLight dirLight;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir){
    vec3 lightDir = normalize(-light.direction);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir =reflect(-lightDir,normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    vec3 ambient= light.ambient * material.ambient;
    vec3 diffuse=light.diffuse * (diff * material.diffuse);
    vec3 specular= light.specular * (spec * material.specular);
    
    return (ambient+diffuse+specular);
}

struct PointLight{
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    vec3 lightDir = normalize(fragPos - light.position); // Reverse the calculation
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir =reflect(-lightDir,normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));

    vec3 ambient= light.ambient * material.ambient;
    vec3 diffuse=light.diffuse * (diff * material.diffuse);
    vec3 specular= light.specular * (spec * material.specular);

    diffuse *= attenuation;
    specular *= attenuation;

    return (ambient+diffuse+specular);
}
struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform SpotLight spotLight;

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(camPos- fragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    float distance = length(camPos - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    vec3 ambient= light.ambient * material.ambient;
    vec3 diffuse=light.diffuse * (diff * material.diffuse);
    vec3 specular= light.specular * (spec * material.specular);

    diffuse  *= intensity;
    specular *= intensity;
    
    return (ambient + diffuse + specular);
}

void main()
{
    vec3 norm=normalize(Normal);
    vec3 viewDir = normalize(camPos-FragPos);
    vec3 directionalResult = vec3(0.0f);
    vec3 spotResult = vec3(0.0f);
    vec3 pointResult = vec3(0.0f);
    //directional
    directionalResult += CalcDirLight(dirLight, norm,viewDir);
    spotResult += CalcSpotLight(spotLight, norm, FragPos,viewDir);
    //points
    for(int i=0; i<NR_POINT_LIGHTS;i++){
        pointResult += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
    }
    //spot

    vec3 result = directionalResult + spotResult + pointResult;
    
    FragColor = texture(texture0,texCoord) * vec4(result,1.0f);
}
