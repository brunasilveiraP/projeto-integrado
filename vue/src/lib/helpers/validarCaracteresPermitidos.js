const filtrarCaracterNumerico = (event, quantity) => {
  let text = event.target.value.toString() + event.key.toString();
  const pattern = /\D/g
  if(quantity) {
    text.length > quantity ? event.preventDefault() : '';
  }
  if (pattern.test(text)) {
    event.preventDefault();
  } else {
    return true;
  }
}

const filtrarCaracterNumericoComVirgula = (event, quantity) => {
    let text = event.target.value.toString() + event.key.toString();
    const pattern = /[0-9]+(,[0-9]+)*,?/g
    if(quantity) {
        text.length > quantity ? event.preventDefault() : '';
    }
    if (pattern.test(text)) {
        event.preventDefault();
    } else {
        return true;
    }
}

const filtrarCaracterString = (event, quantity) => {
  let text = event.target.value.toString() + event.key.toString();
  const pattern = /[^a-zA-Z\s]/g
  if(quantity) {
    text.length > quantity ? event.preventDefault() : '';
  }
  if (pattern.test(text)) {
    event.preventDefault();
  } else {
    return true;
  }
 
}

const filtrarCaracterAlfanumericos = (event, quantity) => {
  let text = event.target.value.toString() + event.key.toString();
  const pattern = /[^a-zA-Z0-9\s]/g
  if(quantity) {
    text.length > quantity ? event.preventDefault() : '';
  }
  if (pattern.test(text)) {
    event.preventDefault();
  } else {
    return true;
  }
}

const removerAcentuacao = (text) => {
  return text.normalize("NFD").replace(/[^a-zA-Z\s]/g, "")
}

export { filtrarCaracterNumerico, filtrarCaracterString, filtrarCaracterAlfanumericos, removerAcentuacao, filtrarCaracterNumericoComVirgula }