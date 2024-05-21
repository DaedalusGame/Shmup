shader_type canvas_item;

uniform float minRadius;
uniform float maxRadius;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float dist = distance(UV, vec2(0.5, 0.5));
	if(dist < 0.5 * minRadius || dist > 0.5 * maxRadius)
	{
		discard;
	}
	COLOR = vec4(1,1,1,1);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
