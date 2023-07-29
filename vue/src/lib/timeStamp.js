import moment from "moment";

const getTimeStamp = function(){

    let date = new Date;

    let year = date.getUTCFullYear();
    let month = ("00" + date.getUTCMonth()).slice(-2);
    let day = ("00" + date.getUTCDate()).slice(-2);
    let hour = ("00" + date.getHours()).slice(-2);
    let minute = ("00" + date.getMinutes()).slice(-2);

    let timeStamp = year+"-"+month+"-"+day+"T"+hour+":"+minute;

    return timeStamp;

};

const formatarTime= (time) => {
    if(time) {
        var timeFormat =  moment(time).format("DD/MM/YYYY")
        return timeFormat;
    }

    if(time === null) return " - "
}

const convertTimeStamp = function(input){

  let dateTimeZone = input + "+0000"

  return new Date(dateTimeZone).toLocaleString('pt-BR', {timeStyle: "short", dateStyle: "short"})

};

export {getTimeStamp, convertTimeStamp, formatarTime};