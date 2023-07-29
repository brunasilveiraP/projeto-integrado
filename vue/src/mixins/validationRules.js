import moment from 'moment'

export default {
  data: () => ({
    rules: {
      required: value => !!value || 'Campo obrigatório',
      nameValidation: value => {
        const pattern = /^[a-zA-Z]{1,}(?: [a-zA-Z]+){1,}[a-zA-Z\u00C0-\u00FF ]+/i;
        return pattern.test(value) || 'Digite ao menos um nome e um sobrenome';

      },
      email: value => {
        const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        const pattern2 = /^([a-zA-z0-9]){1,}$/;
        return pattern.test(value) || pattern2.test(value) || 'O e-mail informado é inválido.'
      },
      cep: value => {
          if(value === "" || value === null) return true
          
          if (
              value === "00000-000" ||
              value === "11111-111" ||
              value === "22222-222" ||
              value === "33333-333" ||
              value === "44444-444" ||
              value === "55555-555" ||
              value === "66666-666" ||
              value === "77777-777" ||
              value === "88888-888" ||
              value === "99999-999"
          )
              return "O CEP informado é inválido";

          const pattern = /^[0-9]{5}-[0-9]{3}$/;
          return pattern.test(value) || "O CEP informado é inválido";
      },
        
      date: (stringData) => {
        if (!stringData) return true
        let dadosData = stringData.split('/')

        if (dadosData.length !== 3) return 'Data inválida'

        let ano = Number(dadosData[2])
        let mes = Number(dadosData[1])
        let dia = Number(dadosData[0])

        if (!Date.parse(ano + '-' + mes + '-' + dia)) return 'Data inválida'
        else if (mes === 2 && dia > 29) return 'Data inválida'
        else if ((mes === 4 || mes === 6 || mes === 9 || mes === 11) && dia > 30)
          return 'Data inválida'
        else if (dia > 31) return 'Data inválida'
        else return true
      },
      compararData: value => {
        const arrData = value.split('/');
        const stringFormatada = arrData[1] + '-' + arrData[0] + '-' + arrData[2];
        const dataFormatada = new Date(stringFormatada).getTime();
        const dataAtual = new Date().getTime()
        if (dataFormatada < dataAtual) return 'A data nao pode ser inferiror a data atual.'
      },
      telefone: value => {
        if (value) {
          const pattern = /^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{4}\-[0-9]{4}$/;
          const pattern2 = /^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$/;
          return pattern.test(value) || pattern2.test(value) || 'O telefone informado é inválido'
        }
        else
          return true;
      },
    },
  }),

  methods: {
    valorMask(v) {
      if (v) {
        let items = [];

        if (v.length === 1)
          return ['#']

        if (v.length >= 4) {
          let hashTags = [];
          for (let index = 3; index < v.length; index++) {
            items.push('#')
            hashTags = items.join('');
          }
          return [`${hashTags},##`]
        }
        else
          return ['#,##'];
      }
      return null
    },
    cpf(cpf) {
      if (this.inputData.tipoPessoa === 2)
        return true;

      if (cpf != null) {
        let initialCpf = cpf;
        cpf = cpf.replace(/[^\d]+/g, '')

        if (
          cpf.length !== 11 ||
          cpf === '00000000000' ||
          cpf === '11111111111' ||
          cpf === '22222222222' ||
          cpf === '33333333333' ||
          cpf === '44444444444' ||
          cpf === '55555555555' ||
          cpf === '66666666666' ||
          cpf === '77777777777' ||
          cpf === '88888888888' ||
          cpf === '99999999999'
        )
          return 'O CPF informado é inválido'

        let add = 0
        for (var i = 0; i < 9; i++) add += parseInt(cpf.charAt(i)) * (10 - i)
        let rev = 11 - (add % 11)
        if (rev === 10 || rev === 11) rev = 0
        if (rev !== parseInt(cpf.charAt(9))) return false || 'O CPF informado é inválido'

        add = 0
        for (var j = 0; j < 10; j++) add += parseInt(cpf.charAt(j)) * (11 - j)
        rev = 11 - (add % 11)
        if (rev === 10 || rev === 11) rev = 0

        const pattern = /^\d{3}\.\d{3}\.\d{3}\-\d{2}$/;

        if (rev !== parseInt(cpf.charAt(10)) || !pattern.test(initialCpf))
          return false || 'O CPF informado é inválido'
        else
          return true
      }
      return true
    },
    cnpj(cnpj) {
      if (this.inputData.tipoPessoa === 1)
        return true
        
        if(cnpj === null || cnpj === "") return  true;

      if (cnpj) {
        let cnpjInitial = cnpj
        cnpj = cnpj.replace(/[^\d]+/g, '')

        // Elimina CNPJs invalidos conhecidos
        if (
          cnpj === '00000000000000' ||
          cnpj === '11111111111111' ||
          cnpj === '22222222222222' ||
          cnpj === '33333333333333' ||
          cnpj === '44444444444444' ||
          cnpj === '55555555555555' ||
          cnpj === '66666666666666' ||
          cnpj === '77777777777777' ||
          cnpj === '88888888888888' ||
          cnpj === '99999999999999'
        )
          return "O CNPJ informado é inválido"
        // Valida DVs
        let tamanho = cnpj.length - 2
        let numeros = cnpj.substring(0, tamanho)
        let digitos = cnpj.substring(tamanho)
        let soma = 0
        let pos = tamanho - 7
        for (var i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--
          if (pos < 2) pos = 9
        }
        let resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11)
        if (resultado !== digitos.charAt(0)) return false || 'O CNPJ informado é inválido'
        tamanho = tamanho + 1
        numeros = cnpj.substring(0, tamanho)
        soma = 0
        pos = tamanho - 7
        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--
          if (pos < 2) pos = 9
        }
        const pattern = /^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/;

        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11)
        if (resultado !== digitos.charAt(1) || !pattern.test(cnpjInitial))
          return false || 'O CNPJ informado é inválido'
        else
          return true
      }
      return true
    },
  }

}

