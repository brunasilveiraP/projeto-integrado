export default {
    data: () => ({
        inputData : {},
        valid: true,
    }),

    watch: {
        resetTrigger(){
            this.$refs.form.reset();
        },
    },

    props:["resetTrigger", "value"],

    methods: {
        closeModal(){
            this.$refs.form.reset();
            this.inputData = {};
            this.$emit("input", false);
        },
        submitForm(){
            if(this.$refs.form.validate()){
                this.$emit("submit", this.inputData);
            }
        }
    }

}