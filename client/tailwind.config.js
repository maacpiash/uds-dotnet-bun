/** @type {import('tailwindcss').Config} */
export default {
    content: ['./src/**/*.{html,svelte}'],
    theme: {
        extend: {},
    },
    plugins: [require('daisyui')],
}
