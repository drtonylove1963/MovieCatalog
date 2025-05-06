# TaskList-001

## Tasks

- [✅] Re-enable event sourcing with Marten
  - [x] Implement temporary SQL Server-based repository for read operations
  - [x] Configure resilient Marten settings in DependencyInjection.cs
  - [x] Enhance error handling in EventStoreService
  - [x] Create new PostgreSQL user for authentication
  - [x] Resolve remaining PostgreSQL authentication issues with resilient fallback
- [ ] Re-enable asynchronous processing with RabbitMQ
- [ ] Add authentication and authorization
- [ ] Implement more advanced search and filtering capabilities
- [ ] Add caching for improved performance
- [ ] Develop a frontend application

## Guidelines

1. Fully implement each task and fully test each task one-by-one
2. Do not move to next task until project has fully vetted the current task being worked on
3. Before moving to next task, push changes to Git and then begin next task
4. Mark completed tasks with a green check mark (✅)
5. Follow the Error Resolution Workflow:
   - Create Task List (Done)
   - Prioritize Issues
   - Fix one error at a time
   - Test after each fix
   - Revert if needed
   - Document changes
   - Final verification
