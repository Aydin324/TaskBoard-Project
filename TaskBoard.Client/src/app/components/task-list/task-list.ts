import { Component, signal } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { TaskItem, TaskItemDto, TaskStatus } from '../../models/task.model';
import { TaskForm } from '../task-form/task-form';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-task-list',
  imports: [TaskForm],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskList {
  tasks = signal<TaskItem[]>([]);
  editingTask = signal<TaskItem | null>(null);
  creating = signal(false);

  constructor(private taskService: TaskService, private toastr: ToastrService) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getAll().subscribe({
      next: (data) => this.tasks.set(data),
      error: (error) => {
        console.error(error);
        this.toastr.error('Failed to load tasks.');
      },
    });
  }

  getStatusLabel(status: TaskStatus) {
    switch (status) {
      case TaskStatus.Todo:
        return 'Todo';
      case TaskStatus.InProgress:
        return 'In Progress';
      case TaskStatus.Done:
        return 'Done';
      default:
        return '';
    }
  }

  deleteTask(id: number) {
    this.taskService.delete(id).subscribe({
      next: () => {
        this.loadTasks();
        this.toastr.success('Task deleted successfully!');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to delete task.');
      },
    });
  }

  toggleTaskStatus(task: TaskItem) {
    this.taskService.toggleStatus(task).subscribe({
      next: () => {
        this.loadTasks();
        this.toastr.success('Task status updated!');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('Failed to update task status.');
      },
    });
  }

  startCreate() {
    this.creating.set(true);
    this.editingTask.set(null);
  }

  startEdit(task: TaskItem) {
    this.editingTask.set(task);
    this.creating.set(false);
  }

  saveTask(task: TaskItem) {
    if (this.editingTask()) {
      this.taskService.update(this.editingTask()!.id, task as TaskItemDto).subscribe({
        next: () => {
          this.loadTasks();
          this.editingTask.set(null);
          this.toastr.success('Task updated successfully!');
        },
        error: (err) => {
          console.error(err);
          this.toastr.error('Failed to update task.');
        },
      });
    } else {
      this.taskService.create(task as TaskItemDto).subscribe({
        next: () => {
          this.loadTasks();
          this.creating.set(false);
          this.toastr.success('Task created successfully!');
        },
        error: (err) => {
          console.error(err);
          this.toastr.error('Failed to create task.');
        },
      });
    }
  }

  cancelForm() {
    this.editingTask.set(null);
    this.creating.set(false);
  }
}
