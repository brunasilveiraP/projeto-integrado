import moment from 'moment'
import { convertTimeStamp } from "@/lib/timeStamp.js"

const filters = [
  {
    name: 'dateTime',
    handler: function (value) {
      if (!value) return '';
      let dateTime = new Date(value);
      return moment(dateTime).format('DD/MM/YYYY HH:mm');
    },
  },
    {
        name: 'date',
        handler: function (value) {
            if (!value) return '';
            let dateTime = new Date(value);
            return moment(dateTime).format('DD/MM/YYYY');
        },
    },
  {
    name: 'truncate',
    handler: function (value, length) {
      if (!value) return '';
      return value.length <= length
        ? value
        : String(value)
          .slice(0, length)
          .concat(['...']);
    },
  },
  {
    name: 'ativoInativo',
    handler: function (value) {
      return value ? 'ATIVO' : 'INATIVO';
    },
  },
  {
    name: 'sequencialCode',
    handler: function (value) {
      if (value && value.length < 3)
        value = value.padStart(3, '0');
      return value;
    },
  },
    {
        name: "currencyFormatReal",
        handler: function (value) {
            if (!value) return;
            value = value.toString().replace(',', ".")
            var formatter = new Intl.NumberFormat('pt-BR', {
                style: 'currency',
                currency: 'BRL'
            });
            let formatedValue = formatter.format(value);
            return formatedValue;
        }
    },
]

export default {
  install(Vue) {
    filters.forEach((filter) => {
      Vue.filter(filter.name, filter.handler);
    });
  },
};