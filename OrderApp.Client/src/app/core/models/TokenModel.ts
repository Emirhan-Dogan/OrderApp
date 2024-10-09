export class TokenModel{
    success:boolean;
    message:string;
    data:Data;
    constructor(){
        this.success=false
        this.message=''
        this.data= new Data
    }

}

export class Data {
    expiration:string;
    token:string;
    claims:string[];
    refreshToken:string;
    user:any;
    
    constructor(){
        this.expiration=''
        this.token=''
        this.claims=[]
        this.refreshToken=''
    }
}