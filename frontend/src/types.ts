export interface Purpose {
    id: number;
    title: string;
  
  }
  export interface PurposeDTO {
    samplePurposeId: number; 
  
  }
  
  
  export interface Goal {
    id: number;
    title: string;
    
  }
  
  export interface Habit {
    id: number;
    title: string;
    description: string;
    goalId: number;
    
  }
  