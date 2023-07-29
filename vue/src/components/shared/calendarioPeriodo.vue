<template>

  <v-menu
    ref="menu"
    v-model="menu"
    :close-on-content-click="false"
    :return-value.sync="datasSelecionadas"
    transition="scale-transition"
    offset-y
    min-width="auto"
    >
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="dateRangeText"
        :label="label"
        :clearable="clearable"
        @click:clear="limparInput()"       
        :class="required ? 'required' : ''"
        :rules="[...buildRules]"
        :prepend-icon="isIcon ? 'mdi-calendar' : ''"
        hint="DD/MM/AAAA"
        persistent-hint
        readonly
        :disabled="disabled"
        v-bind="attrs"
        v-on="on"
      ></v-text-field>
    </template>
    <v-date-picker 
      v-model="datasSelecionadas"
      range
      no-title 
      scrollable>
      <v-spacer></v-spacer>
      <v-btn text color="primary" @click="menu = false"> Cancel </v-btn>
      <v-btn text color="primary" @click="$refs.menu.save(datasSelecionadas)"> OK </v-btn>
    </v-date-picker>
  </v-menu>

</template>
<script>

import validationRules from "@/mixins/validationRules.js";
import moment from "moment/moment";
import { formatDate } from "@/lib/timeStamp.js";

export default {
  mixins: [validationRules],
  props: {
    value: Array,
    required: { type: Boolean, default: true },
    isBtnClose: { type: Boolean, default: false },
    isIcon: { type: Boolean, default: false },
    label: String,
    clearable: {type: Boolean, default: true},    
    disabled: {type: Boolean, default: false},    
    compareRange: {type: Boolean},
    compareToday: {type: Boolean},
  },
  data: function(){
    return {
      dateRangeText: null,
      menu: false,
      isValidDateToday: true,
      isValidDateRange: true,
      compareRangeRule: (v) => {
        return (this.disabled ||  (v != null && this.isValidDateRange) || (v == null && !this.required)) ||
        "A data final não pode ser menor que a data inicial, altere para continuar"
      },
      compareTodayRule:  (v) => {        
        return (this.disabled || (v != null && this.isValidDateToday) || (v == null && !this.required)) ||
        "A data não pode ser inferior a data atual."
      }   
  }},  
  computed: {
    buildRules() {
      let rulesArray = [];
      if(this.required)
        rulesArray.push(this.rules.required)
      if(this.compareToday)
        rulesArray.push(this.compareTodayRule)
      if(this.compareRange)
        rulesArray.push(this.compareRangeRule)
      return rulesArray;
    },
    datasSelecionadas: {
      get() {
        return this.value
      },
      set(val) {         
        if (val.length === 1) {
          this.$emit('changeValue', [val[0]])
        }
        if (val.length === 2) {
          this.$emit('changeValue', val)
        }
        this.$emit('input', val)
      },
    },    
  },
  methods: {   
    limparInput() {
      this.datasSelecionadas = []
      this.$emit('input', null)
    },    
  },
  watch: {
    value() {
      if (this.datasSelecionadas && this.datasSelecionadas.length === 2) {
        let horaAtual = new Date().getHours() ;
        horaAtual = (horaAtual + 4) * 3600000;
        const startDate = this.datasSelecionadas[0];
        const endDate = this.datasSelecionadas[1];
        
        if(this.compareRange) {
          this.isValidDateRange = !(new Date(endDate).getTime() <  new Date(startDate).getTime());
        }
        if(this.compareToday) {
          this.isValidDateToday = new Date(startDate).getTime() >= (new Date().getTime() - horaAtual)
        }
        const dateInicialFormated = moment(this.datasSelecionadas[0]).format("DD/MM/YYYY")
        const dateFinalFormated = moment(this.datasSelecionadas[1]).format("DD/MM/YYYY")
        this.dateRangeText = `${dateInicialFormated} ~ ${dateFinalFormated}`
      }
      else this.dateRangeText = null;
    }
  },

}
</script>

