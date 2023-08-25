void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;

    // Output to screen
    fragColor = vec4(0.0, 0.0, 0.0, 1.0); //black
    
    fragColor = vec4(1.0, 1.0, 1.0, 1.0); //white
    
    fragColor = vec4(1.0, 0.0, 0.0, 1.0); //red
    
    fragColor = vec4(0.0, 1.0, 0.0, 1.0); //green
    
    fragColor = vec4(0.0, 0.0, 1.0, 1.0); //blue
    
}