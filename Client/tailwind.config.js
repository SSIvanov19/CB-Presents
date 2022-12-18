/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["**/*.razor", "**/*.cshtml", "**/*.html"],
    theme: {
        extend: {
            backgroundImage: {
                'main': 'linear-gradient(to right, #15422C 50% , #133D28 50%);',
                'button': 'repeating-linear-gradient(-45deg, #C7261B 0 41px, #175738 11px 84px);'
            },
            fontFamily: {
                'lexend': ['Lexend', 'sans-serif'],
                'lato': ['Lato', 'sans-serif'],
                'montserrat': ['Montserrat', 'sans-serif'],
                'poppins': ['Poppins', 'sans-serif']
            },
            boxShadow: {
                'button': '0px 0px 21px 6px rgba(199, 38, 27, 0.17);'
            }
        },
    },
    plugins: [],
}