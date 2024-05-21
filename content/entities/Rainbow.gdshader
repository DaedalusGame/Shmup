shader_type canvas_item;

render_mode blend_add;

uniform sampler2D noise_tex;
uniform sampler2D ramp_tex;

void vertex() {
}

void fragment() {
	vec4 c = texture(TEXTURE, UV);
	vec4 c_noise = texture(noise_tex, mod(floor(UV / TEXTURE_PIXEL_SIZE) * 0.25 * TEXTURE_PIXEL_SIZE, 1.0)) + TIME;
	COLOR = c * texture(ramp_tex, vec2(mod(c_noise.r * 0.5,1.0), 0));
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
