export enum TaskStatus {
  Todo = 0,
  InProgress = 1,
  Done = 2,
}

//dto
export interface TaskItemDto {
  title: string;
  details?: string;
  status: TaskStatus;
}

//full TaskItem
export interface TaskItem {
  id: number;
  title: string;
  details?: string;
  status: TaskStatus;
  createdAt: string;
}

//helper
export function mapStatus(value: number): TaskStatus {
  switch (value) {
    case 0:
      return TaskStatus.Todo;
    case 1:
      return TaskStatus.InProgress;
    case 2:
      return TaskStatus.Done;
    default:
      return TaskStatus.Todo;
  }
}
