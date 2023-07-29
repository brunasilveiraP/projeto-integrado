const buscarEnderecoViaCep = async (cep) => {
    const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
    const jsonResponse = await response.json();
    if (!response.ok || jsonResponse.erro) {
        return {
            error: true,
            type: 'error',
            message: 'O CEP informado não foi encontrado!'
        };
    }
    else {
        return jsonResponse;
    }
}

export default buscarEnderecoViaCep;
