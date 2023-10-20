void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;
    
    float red = (1.0 - uv.y) * (1.0 - uv.x);
    float green = uv.y * (1.0 - uv.x);
    float blue = (1.0 - green - red) * (uv.y);
    

    // Time varying pixel color
    vec3 col = vec3(red, green, blue);

    // Output to screen
    fragColor = vec4(col,1.0);
}