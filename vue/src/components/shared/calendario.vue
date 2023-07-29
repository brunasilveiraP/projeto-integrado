<template>
    <div>
        <v-menu
            ref="menu"
            v-model="menu"
            :close-on-content-click="false"
            :return-value.sync="date"
            transition="scale-transition"
            offset-y
            min-width="auto"
        >
            <template v-slot:activator="{ on, attrs }">
                <v-text-field
                    :value="formattedDateYear"
                    :label="label"
                    :clearable="clearable"
                    @click:clear="limparInput()"
                    :rules="required ? [...ruleSelected, rules.required] : [...ruleSelected]"
                    :prepend-icon="isIcon ? 'mdi-calendar' : ''"
                    hint="DD/MM/AAAA"
                    persistent-hint
                    :disabled="disabled"
                    v-bind="attrs"
                    v-on="on"
                ></v-text-field>
            </template>
            <v-date-picker
                @input="$refs.menu.save(date)"
                v-model="date"
                no-title
                :disabled="disabled"
                scrollable
                @change="$emit('dateNotFormated', date)"
            >
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="menu = false"> Cancel </v-btn>
                <v-btn text color="primary" @click="$refs.menu.save(date)"> OK </v-btn>
            </v-date-picker>
        </v-menu>
    </div>
</template>

<script>
import validationRules from "@/mixins/validationRules.js";
import moment from "moment";

export default {
    mixins: [validationRules],
    props: {
        isBtnClose: { type: Boolean, default: false },
        isIcon: { type: Boolean, default: false },
        dateToCompareValidate: { type: String, default: null },
        label: String,
        clearable: {type: Boolean, default: true},
        dataProps: String,
        disabled: {type: Boolean, default: false},
        ruleIndexProps: {type: Number, default: 0},
        required: { type: Boolean, default: true },
    },
    data() {
        return {
            menu: false,
            date: null,
            isValidDate: true,
            ruleSelected: [],
            rulesList: [
                [

                ],
                [
                    (v) =>
                        ((v != null && this.isValidDate) || (v == null && !this.required)) ||
                        "A data final não pode ser menor ou igual que a data inicial, altere para continuar",
                ],
                [
                    (v) =>
                        ((v != null && this.isValidDate) || (v == null && !this.required)) ||
                        "A data não pode ser inferior a data atual.",
                ],
            ],
        };
    },
    watch: {
        dataProps(newDate) {
            this.ruleSelected = this.rulesList[this.ruleIndexProps];
            if(newDate != null && newDate.length > 10) {
                this.date = newDate;
            }
        },
    },
    computed: {
        formattedDateYear() {
            let horaAtual = new Date().getHours() ;
            horaAtual = (horaAtual + 4) * 3600000
            if(this.ruleIndexProps === 1 && this.date != null &&  this.date != 0) {
                const invertDate = this.dateToCompareValidate.split('/').reverse().join('-')
                this.isValidDate = new Date(invertDate).getTime() <  new Date(this.date).getTime()
            }
            if(this.ruleIndexProps === 2) {
                this.isValidDate = new Date(this.date).getTime() >= (new Date().getTime() - horaAtual)
            }
            const dateFormated = this.date != null &&  this.date != 0 ? moment(this.date).format("DD/MM/YYYY") : null
            this.$emit('calendarioDate', dateFormated)
            return dateFormated;
        },
    },
    methods: {
        limparInput() {
            this.date = null
            this.$emit('calendarioDate', null)
            this.$emit('limparInput', null)
        },
        setDate(newDate) {
            this.date = newDate.split('/').reverse().join('-')
            this.$refs.menu.save(this.date);
        },
    },
};
</script>

<style lang="scss">

</style>
